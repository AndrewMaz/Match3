using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour
{
  private void Start()
  {
    Camera.main.aspect = 1080f / 1920f;
  }
}