using Core.Cross.SceneData.Interface;
using UnityEngine;

namespace Core.Demo.Scripts.Model
{
    public class DataTransfer : ISceneData
    {
        public DataTransfer()
        {
        }

        public int IntData { get; set; }

        public Color ColorData { get; set; }

        public string StrData { get; set; }
    }

}
