using System.IO;
using Terraria.ModLoader;
using WebCom.DependencyInjection;

namespace WebCom;

public class WebComMod : Mod
{
	public WebComMod()
	{
		This = this;
        Services = new();
	}

    public override void Unload()
    {
        This = null;
    }

    public SimpleServices Services { get; }

    public static WebComMod This { get; private set; }
}
