using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using WebmilioCommons.Extensions;

namespace WebmilioCommons.UI
{
    public class UIBundle<TUIState, TUILayer> where TUIState : StandardUIState where TUILayer : StandardUILayer
    {
        public delegate int LayerIndexProviderDelegate(List<GameInterfaceLayer> layers);


        public UIBundle(InterfaceScaleType scaleType, LayerIndexProviderDelegate layerIndexProvider, params object[] args)
        {
            BundleName = GetType().Name.SplitOnCapital();
            LayerIndexProvider = layerIndexProvider;

            UI = new UserInterface();
            
            UIState = (TUIState) Activator.CreateInstance(typeof(TUIState), args);
            UIState.Activate();

            UILayer = (TUILayer) Activator.CreateInstance(typeof(TUILayer), UIState, BundleName, scaleType);

            UI.SetState(UIState);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (UIState.Visible)
            {
                UI.Update(gameTime);
            }
        }

        public virtual void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            var resourcesLayerIndex = LayerIndexProvider(layers);

            if (resourcesLayerIndex != -1)
                layers.Insert(resourcesLayerIndex, UILayer);
        }


        public string BundleName { get; }
        public LayerIndexProviderDelegate LayerIndexProvider { get; }

        public UserInterface UI { get; }
        public TUIState UIState { get; }
        public TUILayer UILayer { get; }
    }
}