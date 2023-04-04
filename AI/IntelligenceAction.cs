using System;

namespace WebCom.AI;

public interface IIntelligenceAction
{
    public void Update(IntelligenceAction.Context context);
}

public abstract partial class IntelligenceAction : IIntelligenceAction
{
    public virtual void Update(Context context) { }
    public virtual void Restart() { }

    public string Name { get; init; }
    public virtual bool Done { get; }
}