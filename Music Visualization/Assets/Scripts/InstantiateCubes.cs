using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    GameObject[] _sampleCubes = new GameObject[512];
    public float _maxScale;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< 512; i++){
            GameObject _instanceSampleCube = (GameObject)Instantiate (_sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;// sit at center of spawn
            _instanceSampleCube.transform.parent = this.transform; // set the parent of the cube to be the instantiate cube object
            _instanceSampleCube.name = "SampleCube" + i; // rename to make it easier to track our cubes
            this.transform.eulerAngles = new Vector3 (0, -0.703125f * i, 0); // rotate our object
            _instanceSampleCube.transform.position = Vector3.forward * 500; // move cube forward 100 units (forms the circle)
            _sampleCubes [i] = _instanceSampleCube;

        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i< 512; i++){
            if(_sampleCubes != null) {
                // Every frame we will do a local scale on our cubes (we modify values of the cubes individually)
                // we keep the x and z (width and depth) of the object to be a static 10 units
                // we scale the y (height) based on the value given to us by the audio sample
                _sampleCubes[i].transform.localScale = new Vector3(5, (AudioPeer._samples[i] * _maxScale) + 2, 5);

            }
        }   
    }
}
