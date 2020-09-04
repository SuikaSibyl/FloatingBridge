using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FloatingBridge
{
    [RequireComponent(typeof(Rigidbody))]
    public class FloatingObject : MonoBehaviour
    {
        private Rigidbody rd;
        private Transform tf;
        [SerializeField] float torque = 1;
        public float force = 10;

        public GameObject reference_pivot;
        public GameObject referencce_x;
        public GameObject reference_y;
        public GameObject reference_z;

        // Start is called before the first frame update
        void Start()
        {
            rd = this.GetComponent<Rigidbody>();
            tf = this.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            float ff = getFloatForce();
            rd.AddForce(new Vector3(0, Time.deltaTime, 0) * ff * force);
            Vector3 torque_direction =  Vector3.Normalize(Vector3.Cross(tf.up,Vector3.up));
            float torque_cos = Vector3.Dot(Vector3.Normalize(tf.up),Vector3.up);
            float torque_sin = Mathf.Sqrt(1-Mathf.Pow(torque_cos, 2));
            float torque_strenth = ff * torque_sin;
            rd.AddTorque(Time.deltaTime * torque_direction * torque_strenth * torque);
            Debug.Log(tf.up);
        }

        float getFloatForce()
        {
            int cut = 20;
            int count = 0;
            int total = 0;
            Vector3 pos1 = reference_pivot.transform.position;
            Vector3 pos2 = referencce_x.transform.position;
            Vector3 pos3 = reference_y.transform.position;
            Vector3 pos5 = reference_z.transform.position;
            float d0 = pos1.y;
            float dx = ((pos2 - pos1) / cut).y;
            float dy = ((pos3 - pos1) / cut).y;
            float dz = ((pos5 - pos1) / cut).y;
            for (int i = 0; i < cut; i++)
            {
                for (int j = 0; j < cut; j++)
                {
                    for (int k = 0; k < cut; k++)
                    {
                        float y = d0 + i * dx + j * dy + k * dz;
                        if (y < 0) 
                        {
                            count++;
                        }
                        total++;
                    }
                }
            }
            float result = count * 1.0f / total;
            return result;
        }
    }

}