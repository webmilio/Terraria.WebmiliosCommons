using System.IO;
using Terraria.ModLoader;

namespace WebCom;

public class WebComMod : Mod
{
	public WebComMod()
	{
		Instance = this;
	}

    public override void Unload()
    {
        Instance = null;
    }

    internal static WebComMod Instance { get; private set; }
}
