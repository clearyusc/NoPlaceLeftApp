using System;

namespace App2.Models
{
    public enum MinistryActionType { PrayedFor, TestimonyShare, GospelShare };
    public class MinistryAction
    {
        public MinistryActionType Type { get; set; }
        public DateTime Timestamp { get; set; }
        public MinistryAction(MinistryActionType _type)
        {
            Type = _type;
        }
    }
}