using AnchorModeling.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AnchorModeling.QueryExtensions
{
    public class AnchorValue : ICloneable
    {
        public int AnchorId { get; set; }
        public int TransactionId { get; set; }
        public int? CloseTransactionId { get; set; }
        public bool IsTemporary { get; set; }
        public DateTime ApplicationTime { get; set; }
        public Type AttributeType { get; set; }
        public PropertyInfo AnchorAttributePropertyInfo { get; set; }
        public Transaction Transaction { get; set; }
        public Transaction CloseTransaction { get; set; }

        public string StringValue { get; set; }
        public double DoubleValue { get; set; }
        public long LongValue { get; set; }
        public decimal DecimalValue { get; set; }
        public bool BoolValue { get; set; }
        public DateTime TimeValue { get; set; }

        public Guid GuidValue { get; set; }

        //if tie
        public bool IsTie { get; set; }
        public Type TieValueType { get; set; }
        public int ToId { get; set; }

        public object Clone() => MemberwiseClone();

        public override string ToString()
        {
            try
            {
                Type propType = !IsTie ? AnchorAttributePropertyInfo.PropertyType
                    : TieValueType;

                if (propType.Equals(typeof(int)) || propType.Equals(typeof(long))
                    || propType.Equals(typeof(byte)))
                {
                    return LongValue.ToString();
                }
                else
                    if (propType.Equals(typeof(float)) || propType.Equals(typeof(double)))
                {
                    return DoubleValue.ToString();
                }
                else
                    if (propType.Equals(typeof(decimal)))
                {
                    return DecimalValue.ToString();
                }
                else
                    if (propType.Equals(typeof(string)))
                {
                    return StringValue;
                }
                else
                    if (propType.Equals(typeof(bool)))
                {
                    return BoolValue.ToString();
                }
                else
                    if (propType.Equals(typeof(DateTime)))
                {
                    return TimeValue.ToString();
                }
                else
                    if (propType.Equals(typeof(Guid)))
                {
                    return TimeValue.ToString();
                }
                else
                {
                    return base.ToString();
                }
            }
            catch
            {
                //TODO : write to logger
                return null;
            }
        }
    }
}