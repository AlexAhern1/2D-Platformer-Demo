using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class InterfaceReference<TObject, TInterface> where TObject : Object where TInterface : class
    {
        public TObject Object;
        private TInterface _interfaceComponent;

        public TInterface Interface { get
            {
                if (_interfaceComponent == null)
                {
                    if (Object == null)
                    {
                        Logger.Error($"No object to retreive interface - {typeof(TInterface)} from!");
                    }

                    else if (Object is GameObject gameObj)
                    {
                        _interfaceComponent = gameObj.GetComponent<TInterface>();
                        if (_interfaceComponent == null) Logger.Error($"Cannot retreive {typeof(TInterface)} interface from {Object}({typeof(TObject)})!");
                    }
                    else
                    {
                        _interfaceComponent = Object as TInterface;
                        if (_interfaceComponent == null) Logger.Error($"{Object} does not inherit {typeof(TInterface)}!");
                    }
                }
                return _interfaceComponent;
            } 
        }
    }
}