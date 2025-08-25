using UnityEngine;
using System.Collections;

public class GameUIRoot : MonoBehaviour {
	[Tooltip("每个大场景中仅限一个")]
	public UIPanelType firstPanel;
	// Use this for initialization
	void Start () {
        UIManager.Instance.Show(firstPanel);
	}
	

}
