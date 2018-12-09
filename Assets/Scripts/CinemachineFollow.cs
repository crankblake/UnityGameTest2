using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine.Examples
{


    public class CinemachineFollow : MonoBehaviour
    {
        CinemachineVirtualCamera vcam;
        float nextTimeToSearch = 0f;
        private void Start()
        {
            //vcam = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();
            vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            //var brain = GameObject.Find("Main Camera").AddComponent<CinemachineBrain>();
            //brain.m_ShowDebugText = true;
            //brain.m_DefaultBlend.m_Time = 1;

            // vcam = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();
            // vcam.m_LookAt = GameObject.Find("Cube").transform;
            //  vcam.m_Priority = 10;
            //  vcam.gameObject.transform.position = new Vector3(0, 1, 0);
            if (vcam.m_Follow == null)
            {
              //  Debug.Log("CAM IS NULL!");
            }
            if (vcam.m_Follow != null)
            {
               // Debug.Log("CAM IS NOT NULL!");
            }
            // vcam.m_Follow = GameObject.Find("Player").transform;
        }
        void Update()
        {
            if (vcam.m_Follow == null)
            {
               // Debug.Log("CAM IS NULL!");
                FindPlayer();
                //vcam.m_Follow = GameObject.Find("Player").transform;
                //vcam.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (vcam.m_Follow != null)
            {
               // Debug.Log("CAM IS NOT NULL!");
            }
        }
        void FindPlayer()
        {
            if (nextTimeToSearch <= Time.time)
            {
                GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
                if (searchResult != null)
                {
                    //vcam.m_Follow = GameObject.Find("Player").transform;
                    vcam.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
                    //target = searchResult.transform;
                }
                nextTimeToSearch = Time.time + 0.5f;
            }
            else
            {
               // Debug.Log("not time to search");
            }
        }

    }
}

        