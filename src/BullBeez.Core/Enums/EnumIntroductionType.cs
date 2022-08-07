using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BullBeez.Core.Enums
{
    [DataContract]
    public enum EnumIntroductionType
    {
        /// <summary>
        /// SaveChanges işleminde kalıcı olarak silmek için kullanılır.
        /// </summary>
        [EnumMember]
        IntType = 1,

        /// <summary>
        /// Satırda -1 olarak işaretlenir ve mantıksal silindiği anlamına gelir.
        /// </summary>
        [EnumMember]
        StringType = 2,

        /// <summary>
        /// Aktif bir kayıttır.
        /// </summary>
        [EnumMember]
        DatetimeType = 3,

        /// <summary>
        /// Kullanıcı tarafından görülüp tekrar aktif yapılabilecek bir kayıttır.
        /// </summary>
        [EnumMember]
        OnOffType = 4,

        /// <summary>
        /// Kullanıcı tarafından görülüp tekrar aktif yapılabilecek bir kayıttır.
        /// </summary>
        [EnumMember]
        BoolType = 5,

        /// <summary>
        /// Kullanıcı tarafından görülüp tekrar aktif yapılabilecek bir kayıttır.
        /// </summary>
        [EnumMember]
        IntListType = 6,

        /// <summary>
        /// Kullanıcı tarafından görülüp tekrar aktif yapılabilecek bir kayıttır.
        /// </summary>
        [EnumMember]
        Matrix = 7,

        /// <summary>
        /// Kullanıcı tarafından görülüp tekrar aktif yapılabilecek bir kayıttır.
        /// </summary>
        [EnumMember]
        SplitString = 8
    }
}