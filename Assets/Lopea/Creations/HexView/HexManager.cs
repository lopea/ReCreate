using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

namespace Lopea.ReCreate.HexView
{
    public class HexManager : MonoBehaviour
    {
        List<GameObject> Objects;

        [SerializeField]
        GameObject prefab;

        [SerializeField]
        int length = 10;

        [SerializeField]
        float speed = 10, zCutoff = -5;

        [SerializeField]
        float space = 5;
        void Start()
        {
            Objects = new List<GameObject>();
            for (int i = 0; i < length; i++)
            {
                var inst = Instantiate(prefab, new Vector3(0, 0, 5 + i * space), Quaternion.Euler(-90, 0, 180), transform);
                Objects.Add(inst);
            }
        }
        private float GetNextZValue()
        {
            float max = float.MinValue;
            for (int i = 0; i < Objects.Count; i++)
            {
                if (max < Objects[i].transform.position.z)
                    max = Objects[i].transform.position.z;
            }
            return max;
        }
        void Update()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].transform.Translate(new Vector3(0, 0, -speed) * Time.deltaTime, Space.World);

                if (Objects[i].transform.position.z < zCutoff)
                {
                    float max = GetNextZValue() + space;

                    var pos = Objects[i].transform.position;
                    pos.z = max;
                    Objects[i].transform.position = pos;
                }
            }
        }
    }

}