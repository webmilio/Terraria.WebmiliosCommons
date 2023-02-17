using System;

namespace WebCom.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SaveAttribute : Attribute { }
