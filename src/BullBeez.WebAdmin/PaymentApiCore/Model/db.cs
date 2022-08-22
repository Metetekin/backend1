using BullBeez.WebAdmin.RequestDTO;
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
        private string _connectionString = "Server=localhost; Port=3306;Database=bullbee1_21032022;User Id = bullbee1_api; Password=kadir235.,";
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
                 DATE_FORMAT(sas.Inserteddate, '%Y-%m-%d %T') Inserteddate, case when sas.ispayment = 1 then 'Ödeme Yapılmadı' else 'Ödeme Yapıldı' end as IsPayment FROM serviceanswer as sas
                 left join service as sss on sas.serviceid = sss.Id inner join questionoptions as qqq on sas.questionoptionsid = qqq.id
                 inner join questions as qeq on qeq.id = qqq.questionid inner join companyandperson as cap on cap.id = sas.companyandpersonid
                 where sas.Guid = @Guid", new { Guid = id }).ToList();

            return response;
        }

        public List<ServicePayment> GetServicePayment()
        {
            var response = _conn.Query<ServicePayment>(@"SELECT cap.NameOrTitle,cap.EmailAddress,cap.GSM,sss.ServiceName,sss.Amount,qeq.Question,qqq.Option, sas.Guid,
                (select textdata from serviceanswer where companyandpersonid = sas.companyandpersonid and QuestionoptionsId = 0 limit 1)TextCevap,
                 DATE_FORMAT(sas.Inserteddate, '%Y-%m-%d %T') Inserteddate, case when sas.ispayment = 1 then 'Ödeme Yapılmadı' else 'Ödeme Yapıldı' end as IsPayment FROM serviceanswer as sas
                 left join service as sss on sas.serviceid = sss.Id inner join questionoptions as qqq on sas.questionoptionsid = qqq.id
                 inner join questions as qeq on qeq.id = qqq.questionid inner join companyandperson as cap on cap.id = sas.companyandpersonid").ToList();

            return response;
        }

        public List<PackagePayment> GetPackagePayment()
        {
            var response = _conn.Query<PackagePayment>(@"
                        SELECT cap.NameOrTitle,cap.EmailAddress,cap.GSM,p.PackageName,pp.Amount,DATE_FORMAT(pp.Inserteddate, '%Y-%m-%d %T') Inserteddate, 
                         case when pp.UpdatedBy = 'Manuel Kayıt' then 'Manuel Kayıt' else 'Ödeme Yapıldı' end as IsPayment
                        from packagepayments pp
                        inner join Packages p on pp.PackageId=p.Id
                        inner join companyandperson as cap on cap.id = pp.companyandpersonid").ToList();

            return response;
        }

        public List<SendNotificationUserListResponse> SendUserNotification(SendNotificationRequest request)
        {
            try
            {
                var query = @"Select * from (SELECT cp.Id,cp.NameOrTitle,cp.UserName,YEAR(cp.BirthDay)BirthDay,
                                    (Select FirebaseToken from token where CompanyAndPersonId = cp.Id and Rowstatu = 1 order by Id desc limit 1)FirebaseToken,
                                    (Select LENGTH(DeviceId) from token where CompanyAndPersonId = cp.Id and Rowstatu = 1 order by Id desc limit 1)DeviceIdCount,
                                    (Select OccupationId from companyandpersonoccupation where CompanyAndPersonId = cp.Id and Rowstatu = 1 order by Id desc limit 1)OccupationId
                                     FROM companyandperson cp where 1=1 ";

                if (request.CompanyAndPersonId > 0)
                {
                    query += " and cp.Id =" + request.CompanyAndPersonId;
                    query += ") tbl where 1=1 ";
                }
                else
                {
                    if (request.ProfileType == 1)
                    {
                        query += " and cp.ProfileType =" + request.ProfileType;
                        if (request.Gender != -1)
                        {
                            query += " and cp.Gender = @Gender";
                        }

                        if (request.EndYear != 0)
                        {
                            query += " and @StartBirthDay >= YEAR(cp.BirthDay) and  YEAR(cp.BirthDay) >= @EndBirthDay";
                        }


                        query += ") tbl where 1=1 ";


                        if (request.Occupations != null)
                        {
                            query += " and OccupationId in (" + string.Join(",", request.Occupations) + ")";
                        }

                    }
                    else if (request.ProfileType == 2)
                    {
                        query += " and cp.ProfileType =" + request.ProfileType;

                        if (request.CompanyType != null)
                        {
                            query += " and cp.CompanyTypeId in (" + string.Join(",", request.CompanyType) + ")";
                        }

                        query += ") tbl where 1=1 ";
                    }
                    else
                    {
                        if (request.CompanyType != null)
                        {
                            query += " and cp.CompanyTypeId in (" + string.Join(",", request.CompanyType) + ")";
                        }
                        if (request.Gender != -1)
                        {
                            query += " and cp.Gender = @Gender";
                        }

                        if (request.EndYear != 0)
                        {
                            query += " and @StartBirthDay >= YEAR(cp.BirthDay) and  YEAR(cp.BirthDay) >= @EndBirthDay";
                        }

                        query += ") tbl where 1=1 ";

                        if (request.Occupations != null)
                        {
                            query += " and OccupationId in (" + string.Join(",", request.Occupations) + ")";
                        }
                    }


                    if (request.PhoneMark == 1)
                    {
                        query += " and DeviceIdCount = 36";
                    }
                    if (request.PhoneMark == 2)
                    {
                        query += " and DeviceIdCount = 16";
                    }
                }
                

                var response = _conn.Query<SendNotificationUserListResponse>(query, new { Gender = request.Gender, StartBirthDay = DateTime.Now.AddYears(-request.StartYear).Year, EndBirthDay = DateTime.Now.AddYears(-request.EndYear).Year }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
            
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

        public void InsertLog(string log, string guid, string UserDetail, string RequestDetail)
        {
            var userInfo = _conn.Execute("insert into log(Hata,UserDetail,RequestDetail,Guid) values (@Hata,@UserDetail,@RequestDetail,@Guid)", new
            {
                Hata = log,
                UserDetail = UserDetail,
                RequestDetail = RequestDetail
            ,
                Guid = guid
            });

        }

        public void InsertPackagePayment(int PackageId, int CompanyAndPersonId)
        {
            var userInfo = _conn.Execute(@"INSERT INTO packagepayments
                           (CompanyAndPersonId,
                            PackageId,
                            TextData,
                            IsPayment,
                            Amount,
                            Guid,
                            ContractConfirmation,
                            RowStatu,
                            InsertedBy,
                            InsertedIp,
                            InsertedDate,
                            UpdatedBy,
                            UpdatedIp)
                        VALUES
                        ( @CompanyAndPersonId,
                          @PackageId,
                          '',
                          1,
                          0,
                          @Guid,
                          1,
                          1,
                          'Manuel',
                          '',
                          @InsertedDate,
                          'Manuel Kayıt',
                          '')", new
            {
                CompanyAndPersonId = CompanyAndPersonId,
                PackageId = PackageId,
                Guid = Guid.NewGuid(),
                InsertedDate = DateTime.Now
            });

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