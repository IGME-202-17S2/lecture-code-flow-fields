using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField : MonoBehaviour {

	public Texture2D source;
	public static Vector3[,] flows = new Vector3[64,64];

	// Use this for initialization
	void Start () {
		// https://docs.unity3d.com/560/Documentation/ScriptReference/Texture2D.GetPixels32.html
		// https://docs.unity3d.com/560/Documentation/ScriptReference/Color32.html
		Color32[] pixels = source.GetPixels32();
		// pixels is a flat array of Color32, reading left to right, bottom to top

		int flatIndex = 0;
		for (int z = 0; z < 64; z++) {
			for (int x = 0; x < 64; x++) {
				// get the next pixel
				Color32 pixel = pixels [flatIndex];

				// convert it into a flow field Vector3
				FlowField.flows [x, z] = new Vector3(
					(pixel.r - 128)/255f, // red channel is x
					(pixel.b - 128)/255f, // blue channel is y
					(pixel.g - 128)/255f  // green channel is z
				);

				// prepare for the next pixel
				flatIndex++;
			}
		}

		// by the end of this:
		// FlowField.flows[0,0] is the lower left flowfield vector
		// FlowField.flows[63, 63] is the upper right flowfield vector
	}
	
	public static Vector3 GetFlowAtPos(Vector3 pos) {
		// convert a world position to be indices into the flows 2d-array
		// clamping keeps each index in bounds for the 2d-array
		// casting as an (int) truncates the decimal portion of the float
		int x = (int)Mathf.Clamp (pos.x, 0, 63);
		int z = (int)Mathf.Clamp (pos.z, 0, 63);

		// return the flow Vector3 at that position in the flows 2d-array
		return FlowField.flows [x, z];
	}

	void OnDrawGizmosSelected() {
		/*
		 *    b1 --- b2
		 *     |      |
		 *     |      |
		 *    b4 --- b3
		 */
		Vector3 b1 = new Vector3 (-0.1f, 0, 0.1f);
		Vector3 b2 = new Vector3 (0.1f, 0, 0.1f);
		Vector3 b3 = new Vector3 (0.1f, 0, -0.1f);
		Vector3 b4 = new Vector3 (-0.1f, 0, -0.1f);

		Vector3 start = new Vector3();
		Vector3 end = new Vector3();
		start.y = 1f;

		// iterate through every position in the flow field
		for (int x = 0; x < 64; x++) {
			for (int z = 0; z < 64; z++) {
				// set the start position (center of a 1x1 section of the world)
				start.x = 0.5f + x;
				start.z = 0.5f + z;
				// grab the vector at that position
				Vector3 flow = FlowField.flows [x, z];
				// calculate the end position
				end = start + flow;
				// first line is direction
				Debug.DrawLine (start, end, Color.white, 0);
				// two lines make an X over the start
				Debug.DrawLine (start + b1, start + b3, Color.red, 0);
				Debug.DrawLine (start + b2, start + b4, Color.red, 0);
			}
		}
	}
}
