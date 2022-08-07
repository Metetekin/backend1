using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.BaseEntities
{
    public class EntBaseAdvanced : EntityBase
    {
        public virtual string InsertedBy { get; set; }
        public virtual string InsertedIp { get; set; }
        public virtual DateTime? InsertedDate { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual string UpdatedIp { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }

        public EntBaseAdvanced()
        {
            if (Id > 0)
            {
                this.RowStatu = EnumRowStatusType.Active;
                this.UpdatedDate = DateTime.Now;
                //this.InsertedBy = ApplicationContext.Get<long?>(ApplicationContextCode.UserId) == null
                //    ? "-1"
                //    : ApplicationContext.Get<long>(ApplicationContextCode.UserId).ToString();

                //this.InsertedIp = ApplicationContext.Get<string>(ApplicationContextCode.RequestIp) == null
                //    ? "127.0.0.1"
                //    : ApplicationContext.Get<string>(ApplicationContextCode.RequestIp);
            }
            else
            {
                this.RowStatu = EnumRowStatusType.Active;
                this.InsertedDate = DateTime.Now;
                //this.InsertedBy = ApplicationContext.Get<long?>(ApplicationContextCode.UserId) == null
                //    ? "-1"
                //    : ApplicationContext.Get<long>(ApplicationContextCode.UserId).ToString();

                //this.InsertedIp = ApplicationContext.Get<string>(ApplicationContextCode.RequestIp) == null
                //    ? "127.0.0.1"
                //    : ApplicationContext.Get<string>(ApplicationContextCode.RequestIp);
            }


            //this.InsertedBy = ApplicationContext.Get<long?>(ApplicationContextCode.UserId) == null
            //    ? "-1"
            //    : ApplicationContext.Get<long>(ApplicationContextCode.UserId).ToString();

            //this.InsertedIp = ApplicationContext.Get<string>(ApplicationContextCode.RequestIp) == null
            //    ? "127.0.0.1"
            //    : ApplicationContext.Get<string>(ApplicationContextCode.RequestIp);

            //this.InsertedDate = DateTime.Now;
        }

        public void Update()
        {
            this.UpdatedBy = "1";

            this.UpdatedIp = "127.0.0.1";

            this.UpdatedDate = DateTime.Now;
        }
    }
}

