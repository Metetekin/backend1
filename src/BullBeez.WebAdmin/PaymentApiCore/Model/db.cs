using BullBeez.WebAdmin.ResponseDTO;

using Dapper;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class db
    {
        private readonly MySqlConnection _conn;
        private string _connectionString = "Server=185.210.93.202; Port=6687;Database=bullbez4;User Id=dsis;Password=Tk45azx!;";
        //private string _connectionString = "server=localhost;user id=bullbee1_api;password=Tk45azx!;persistsecurityinfo=True;port=3306;database=bullbee1_api;SslMode=none;";
        public db()
        {

            _conn = new MySqlConnection(_connectionString);
        }
        public CardDetailModel GetUser(int userId, int servisId)
        {
            var userInfo = _conn.QueryFirst<User>("SELECT * FROM CompanyAndPerson where Id=@Id", new { Id = userId });

            var userAddress = _conn.QueryFirst<Address>(@"select a.Id,a.CountyId,c.Name CountyName,a.AddressString,cc.Id CityId,cc.Name CityName from address as a
                            inner join County as c on a.CountyId = c.Id
                            inner join City as cc on cc.Id = c.cityId where CompanyAndPersonsId=@CompanyAndPersonId", new { CompanyAndPersonId = userId });

            var serviceInfo = _conn.QueryFirst<Service>("SELECT * FROM Service where Id=@Id", new { Id = servisId });

            CardDetailModel response = new CardDetailModel();
            response.UserId = userId;
            response.ServiceName = serviceInfo.ServiceName;
            response.ServiceId = servisId;
            response.NameOrTitle = userInfo.NameOrTitle;
            response.EmailAddress = userInfo.EmailAddress;
            response.GSM = userInfo.GSM;
            response.Amount = serviceInfo.Amount;
            response.AmountFirst = (serviceInfo.Amount / 100) * 82;
            response.KDV = response.Amount - response.AmountFirst;
            response.Address = userAddress;
            return response;
        }

        public CardDetailModel GetUserPackage(int userId, int packageId)
        {
            var userInfo = _conn.QueryFirst<User>("SELECT * FROM CompanyAndPerson where Id=@Id", new { Id = userId });

            var userAddress = _conn.QueryFirst<Address>(@"select a.Id,a.CountyId,c.Name CountyName,a.AddressString,cc.Id CityId,cc.Name CityName from address as a
                            inner join County as c on a.CountyId = c.Id
                            inner join City as cc on cc.Id = c.cityId where CompanyAndPersonsId=@CompanyAndPersonId", new { CompanyAndPersonId = userId });

            var serviceInfo = _conn.QueryFirst<PackageModel>("SELECT * FROM Packages where Id=@Id", new { Id = packageId });

            CardDetailModel response = new CardDetailModel();
            response.UserId = userId;
            response.ServiceName = serviceInfo.PackageName;
            response.ServiceId = packageId;
            response.NameOrTitle = userInfo.NameOrTitle;
            response.EmailAddress = userInfo.EmailAddress;
            response.GSM = userInfo.GSM;
            response.Amount = serviceInfo.Amount;
            response.AmountFirst = (serviceInfo.Amount / 100) * 82;
            response.KDV = response.Amount - response.AmountFirst;
            response.Address = userAddress;
            return response;
        }

        public void InsertLog(string log)
        {
            var userInfo = _conn.Execute("insert into log(Hata) values (@val)", new { val = log });

        }

        public Log GetLog(string guid)
        {
            var response = _conn.QueryFirst<Log>("SELECT * FROM log where Guid=@Guid", new { Guid = guid });

            return response;
        }

        public List<ServicePayment> GetServicePaymentById(Guid id)
        {
            var response = _conn.Query<ServicePayment>(@"SELECT cap.NameOrTitle,cap.EmailAddress,cap.GSM,sss.ServiceName,sss.Amount,qeq.Question,qqq.Option, sas.Guid,
                (select textdata from serviceanswer where companyandpersonid = sas.companyandpersonid and QuestionoptionsId = 0 limit 1)TextCevap,
                 DATE_FORMAT(sas.Inserteddate, '%Y-%m-%d %T') Inserteddate, case when sas.ispayment = 1 then 'Ödeme Yapılmadı' else 'Ödeme Yapıldı' end as IsPayment FROM `serviceanswer` as sas
                 left join service as sss on sas.serviceid = sss.Id inner join questionoptions as qqq on sas.questionoptionsid = qqq.id
                 inner join questions as qeq on qeq.id = qqq.questionid inner join companyandperson as cap on cap.id = sas.companyandpersonid
                 where sas.Guid = @Guid", new { Guid = id }).ToList();

            return response;
        }

        public List<ServicePayment> GetServicePayment()
        {
            var response = _conn.Query<ServicePayment>(@"SELECT cap.NameOrTitle,cap.EmailAddress,cap.GSM,sss.ServiceName,sss.Amount,qeq.Question,qqq.Option, sas.Guid,
                (select textdata from serviceanswer where companyandpersonid = sas.companyandpersonid and QuestionoptionsId = 0 limit 1)TextCevap,
                 DATE_FORMAT(sas.Inserteddate, '%Y-%m-%d %T') Inserteddate, case when sas.ispayment = 1 then 'Ödeme Yapılmadı' else 'Ödeme Yapıldı' end as IsPayment FROM `serviceanswer` as sas
                 left join service as sss on sas.serviceid = sss.Id inner join questionoptions as qqq on sas.questionoptionsid = qqq.id
                 inner join questions as qeq on qeq.id = qqq.questionid inner join companyandperson as cap on cap.id = sas.companyandpersonid").ToList();

            return response;
        }

        public int DeleteLog(string guid)
        {
            var response = _conn.Execute("Delete FROM log where Guid=@Guid", new { Guid = guid });

            return response;
        }

        public int UpdateLog(string log, string guid, string UserDetail, string RequestDetail)
        {
            var response = _conn.Execute("Update log set Hata =@Hata ,UserDetail=@UserDetail,RequestDetail=@RequestDetail where Guid=@Guid", new
            {
                Hata = log,
                UserDetail = UserDetail,
                RequestDetail = RequestDetail,
                Guid = guid
            });

            return response;
        }

        public int UpdateCreatePayment(Guid createPaymentId, int isPayment)
        {
            var response = _conn.Execute("Update ServiceAnswers IsPayment =@isPayment,UpdatedDate =@UpdatedDate   where Guid=@Guid", new { Guid = createPaymentId, UpdatedDate = DateTime.Now });

            return response;
        }

        public void InsertLog(string log,string guid,string UserDetail, string RequestDetail)
        {
            var userInfo = _conn.Execute("insert into log(Hata,UserDetail,RequestDetail,Guid) values (@Hata,@UserDetail,@RequestDetail,@Guid)", new { Hata = log, UserDetail  = UserDetail , RequestDetail  = RequestDetail 
            ,Guid = guid});

        }

        public void ServiceAnswer(Guid guid, int isPayment)
        {
            var userInfo = _conn.Execute("update serviceanswer set IsPayment = @IsPayment where Guid = @Guid", new
            {
                IsPayment = isPayment,
                Guid = guid
            });

        }

        public void InsertPackagePayments(int userId, int packageId, int IsPayment, decimal amount, int contractConfirmation)
        {
            var userInfo = _conn.Execute(@"insert into log(CompanyAndPersonId,
                                                            PackageId,
                                                            TextData,
                                                            IsPayment,
                                                            Amount,
                                                            Guid,
                                                            ContractConfirmation,
                                                            RowStatu,
                                                            InsertedBy,
                                                            InsertedIp,
                                                            InsertedDate) 
                                                        values (
                                                            @CompanyAndPersonId,
                                                            @PackageId,
                                                            @TextData,
                                                            @IsPayment,
                                                            @Amount,
                                                            @Guid,
                                                            @ContractConfirmation,
                                                            @RowStatu,
                                                            @InsertedBy,
                                                            @InsertedIp,
                                                            @InsertedDate)", new
                    {
                        CompanyAndPersonId = userId,
                        PackageId = packageId,
                        TextData = "",
                        IsPayment = IsPayment,
                        Amount = amount,
                        Guid = Guid.NewGuid().ToString(),
                        ContractConfirmation = contractConfirmation,
                        RowStatu = 1,
                        InsertedBy = "",
                        InsertedIp = "",
                        InsertedDate = DateTime.Now
            });

        }
    }
}