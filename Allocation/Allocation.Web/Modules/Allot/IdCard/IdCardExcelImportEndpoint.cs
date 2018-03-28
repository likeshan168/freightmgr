
using Allocation.Allot.Repositories;

namespace Allocation.Allot.Endpoints
{
    using Allocation.Allot.Entities;
    using Allocation.Allot.IdCard;
    using OfficeOpenXml;
    using RestSharp;
    using Serenity;
    using Serenity.Data;
    using Serenity.Services;
    using Serenity.Web;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MyRow = Entities.IdCardRow;

    [RoutePrefix("Services/Allot/IdCardInfoExcelImport"), Route("{action}")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize]
    public class IdCardInfoExcelImportController : ServiceEndpoint
    {
        [HttpPost]
        public ExcelImportResponse ExcelImport(IUnitOfWork uow, MyExcelImportRequest request)
        {
            request.CheckNotNull();
            Check.NotNullOrWhiteSpace(request.FileName, "filename");

            UploadHelper.CheckFileNameSecurity(request.FileName);

            if (request.ApiId == 0)
            {
                throw new ArgumentOutOfRangeException("filename");
            }

            if (!request.FileName.StartsWith("temporary/"))
                throw new ArgumentOutOfRangeException("filename");

            ExcelPackage ep = new ExcelPackage();
            using (var fs = new FileStream(UploadHelper.DbFilePath(request.FileName), FileMode.Open, FileAccess.Read))
                ep.Load(fs);

            var p = MyRow.Fields;


            var response = new ExcelImportResponse { ErrorList = new List<string>() };

            if (ep.Workbook.Worksheets.Count == 0)
            {
                response.ErrorList.Add("The Excel file doesn't cantain any sheet");
                return response;
            }
            var orderSheet = ep.Workbook.Worksheets[1];

            //先遍历获取所有的订单明细信息
            var idCards = new List<MyRow>();
            for (var row = 2; row <= orderSheet.Dimension.End.Row; row++)
            {
                var name = Convert.ToString(orderSheet.Cells[row, 1].Value ?? string.Empty);
                var cardNo = Convert.ToString(orderSheet.Cells[row, 2].Value ?? string.Empty);
                if (name.IsTrimmedEmpty() || cardNo.IsTrimmedEmpty())
                {
                    continue;
                }
                var tmpRow = new MyRow
                {
                    Name = name,
                    CardNo = cardNo
                };

                idCards.Add(tmpRow);
            }
            var idCardRepository = new IdCardRepository();
            var apiRepository = new ApiRepository();
            var apiEntity = apiRepository.Retrieve(uow.Connection, new RetrieveRequest
            {
                EntityId = request.ApiId
            });
            if (apiEntity == null || apiEntity.Entity == null)
            {
                throw new ArgumentOutOfRangeException("API接口不存在");
            }
            var api = apiEntity.Entity;

            foreach (var idCard in idCards)
            {
                //验证是否已经存在于数据库中，如果存在则直接用数据库中
                var lst = idCardRepository.List(uow.Connection, new ListRequest
                {
                    Criteria = new Criteria("Name") == idCard.Name & new Criteria("CardNo") == idCard.CardNo
                });

                if (lst != null && lst.Entities != null && lst.Entities.Count > 0)
                {
                    var first = lst.Entities.First();
                    //继续重复利用数据中已经存在的数据
                    if (api.ReuseCount > first.ReusedCount)
                    {
                        first.RequestIp = Request.UserHostAddress;
                        first.RequestDate = DateTime.Now;
                        first.Price = api.Price;
                        first.ReusedCount = first.ReusedCount + 1;
                        //更新数据库
                        idCardRepository.Update(uow, new SaveRequest<MyRow>
                        {
                            Entity = first,
                            EntityId = first.Id
                        });

                        response.Updated = response.Updated + 1;
                    }
                    else
                    {
                        //去调用API接口进行验证 更新本地的数据
                        var result = RequestApi(api, first);
                        if (result.Data.Isok == 1)
                        {
                            if (result.Data.Data.Err == 0)
                            {
                                first.AuthenticationType = "实名认证";
                                first.CheckResult = GetMessage(result.Data.Code);
                                first.RequestDate = DateTime.Now;
                                first.RequestIp = Request.UserHostAddress;
                                first.Price = api.Price;
                                first.ReusedCount = 0;
                                //更新数据库
                                idCardRepository.Update(uow, new SaveRequest<IdCardRow>
                                {
                                    Entity = first,
                                    EntityId = first.Id
                                });
                                response.Updated = response.Updated + 1;
                            }
                            else
                            {
                                //这种情况由于会存在个别身份证格式不正确的情况，所有一个有错误，后面的还是可以继续进行的
                                response.ErrorList.Add($"{idCard.Name} 验证失败, {GetMessage(result.Data.Code)}");
                            }
                        }
                        else
                        {
                            //在这种情况就说明之后的验证也不会通过，那么就直接退出，没有必要进行后面的验证
                            response.ErrorList.Add($"验证失败, {GetMessage(result.Data.Code)}");
                            break;
                        }
                    }
                }
                else
                {
                    //去调用API接口进行验证 新增本地的数据，因为不存在
                    var result = RequestApi(api, idCard);
                    if (result.Data.Isok == 1)
                    {
                        if (result.Data.Data.Err == 0)
                        {
                            idCard.AuthenticationType = "实名认证";
                            idCard.CheckResult = GetMessage(result.Data.Code);
                            idCard.RequestDate = DateTime.Now;
                            idCard.RequestIp = Request.UserHostAddress;
                            idCard.Price = api.Price;
                            idCard.ReusedCount = 0;
                            //更新数据库
                            idCardRepository.Create(uow, new SaveRequest<IdCardRow>
                            {
                                Entity = idCard
                            });
                            response.Inserted = response.Inserted + 1;
                        }
                        else
                        {
                            //这种情况由于会存在个别身份证格式不正确的情况，所有一个有错误，后面的还是可以继续进行的
                            response.ErrorList.Add($"{idCard.Name} 验证失败, {GetMessage(result.Data.Code)}");
                        }
                    }
                    else
                    {
                        //在这种情况就说明之后的验证也不会通过，那么就直接退出，没有必要进行后面的验证
                        response.ErrorList.Add($"验证失败, {GetMessage(result.Data.Code)}");
                        break;
                    }
                }
            }
            return response;
        }

        private IRestResponse<ApiResponse> RequestApi(ApiRow api, MyRow row)
        {
            var client = new RestClient(new Uri(api.ApiUrl));
                var request = new RestRequest();
            request.AddParameter("appkey", api.AppKey);
            request.AddParameter("name", row.Name);
            request.AddParameter("cardno", row.CardNo);
            var result = client.Execute<ApiResponse>(request);
            return result;
        }

        private string GetMessage(int code)
        {
            switch (code)
            {
                #region 这个是查询成功的情况
                case 1:
                    return "一致";
                case 2:
                    return "不一致";
                case 3:
                    return "无此身份证号码";
                #endregion
                #region 这个是查询失败的情况
                case 11:
                    return "参数不正确";
                case 12:
                    return "商户余额不足";
                case 13:
                    return "appkey不存在";
                case 14:
                    return "IP被拒绝";
                case 20:
                    return "身份证中心维护中";
                default:
                    return "参数不正确";
                    #endregion
            }
        }
    }
}