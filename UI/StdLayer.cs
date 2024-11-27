using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using WebCom.Constants;

namespace WebCom.UI;

public class StdLayer : GameInterfaceLayer
{
    public UserInterface Interface { get; }
    public UIState State { get; }

    public StdLayer(UIState state) : base(nameof(StdLayer), InterfaceScaleType.UI)
    {
        Interface = new();
        Interface.SetState(state);

        State = state;
        Active = false;
    }

    public void Update(GameTime gameTime)
    {
        Interface.Update(gameTime);
    }

    public void UpdateIfVisible(GameTime gameTime)
    {
        if (Active)
        {
            Update(gameTime);
        }
    }

    public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers, string layerName)
    {
        if (Active)
        {
            var index = layers.FindIndex(l => l.Name.Equals(layerName));

            if (index > -1)
            {
                layers.Insert(index, this);
            }
        }
    }

    protected override bool DrawSelf()
    {
        State.Draw(Main.spriteBatch);

        return true;
    }
}
