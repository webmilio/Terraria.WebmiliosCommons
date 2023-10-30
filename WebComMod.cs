using Terraria.ModLoader;
using WebCom.DependencyInjection;
using WebCom.Resolvers;

namespace WebCom;

public class WebComMod : Mod
{
	public WebComMod()
	{
		This = this;

        GlobalContainers = new();
        
        Services = new()
        {
            PromoteToModContent = true
        };
        Services.MapServices(typeof(WebComMod).Assembly);
	}

    public void ChainServiceContainers(SimpleServices services)
    {
        services.AddContainer(Services);
        GlobalContainers.AddContainer(services);
    }

    public override void Load()
    {
        Services.AddSingleton(new Saving.Saver());
        Services.AddSingleton(new Resolvers.ModsProvider());
    }

    public override void Unload()
    {
        This = null;
    }

    /// <summary>Register your service containers here.</summary>
    public SimpleServices GlobalContainers { get; }

    /// <summary>WebCom's services. You can add this container to yours.</summary>
    public SimpleServices Services { get; }

    public static WebComMod This { get; private set; }
}
