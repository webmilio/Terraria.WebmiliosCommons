using System;
using System.ComponentModel.Design;
using Terraria.ModLoader;
using WebCom.DependencyInjection;

namespace WebCom;

public class WebComMod : Mod
{
	public WebComMod()
	{
		This = this;

        SharedServices = new();
        GlobalContainer = new();
	}

    public override void Load()
    {
        ContentInstance.Register(new Saving.Saver());

        SharedServices.MapServices(this);
    }

    public override void Unload()
    {
        This = null;
    }

    /// <summary>
    ///     Provides common instances to your services by adding this collection as a <see cref="IServiceProvider"/>/<see cref="IServiceContainer"/> to your own.
    ///     Don't register anything on this! Add your service providers/containers to <see cref="GlobalContainer"/>.
    /// </summary>
    public SimpleServices SharedServices { get; }

    /// <summary>Register your service collections here. For base services shared by individual collections, see <see cref="SharedServices"/>.</summary>
    public SimpleServices GlobalContainer { get; }

    public static WebComMod This { get; private set; }
}
