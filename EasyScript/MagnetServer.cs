using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// 作者: TomYuan
public MagnetServer : MonoBehaviour {

	// 磁铁对象
	public List<GameObject> magents;
	public GameObject sliding;

	public Vector3 startPoint;
    public Vector3 endedPoint;

    public float use_tm = 0.0f;
	public bool touched = false;

	// 隐藏所有磁级
	private void HideAllPoles()
    {
        // 所有磁铁不显示
        for (int i = 0; i < magents.Count; i++)
        {
            MagentMono magent = magents[i].GetComponent<MagentMono>();
            for (int j = 0; j < magent.poles.Count; j++)
            {
                magent.poles[j].setVisible(false);
                magent.poles[j].name = "";
            }
        }
    }

    private bool checkHorizonPoles(magent1, k1, magent2, k2) {
    	bool ret = false;

    	float r = 0.5f;
    	PoleMono pole1 = magent1.poles[k1];
    	PoleMono pole2 = magent2.poles[k2];

    	if (pole1.pole_type == pole2.pole_type)
    	{
    		if (Mathf.Abs(magent1.gameObject.transform.position.y - 
    				magent2.gameObject.transform.position.y) > 2 * r)
            {
                if (pole1.pole_type == (int)POLE_TYPE.HORIZONTAL)
                {
                    // 水平
                    if (pole1.transform.localPosition.y > 0)
                    {
                        // Up
                        if (pole2.transform.localPosition.y < 0)
                        {
                            if (pole1.transform.position.y - pole2.transform.position.y > -0.3f &&
                                pole1.transform.position.y - pole2.transform.position.y < -0.0f &&
                                Mathf.Abs(pole1.transform.position.x - pole2.transform.position.x) < 0.2f &&
                                !pole1.obj && !pole2.obj)
                            {
                                pole1.setVisible(true);
                                pole2.setVisible(true);

                                pole1.name = magent2.gameObject.name;
                                pole2.name = magent1.gameObject.name;
                                ret = true;
                            }
                        }
                    }
                    else
                    {
                        if (pole2.transform.localPosition.y > 0)
                        {
                            // Down
                            if (pole1.transform.position.y - pole2.transform.position.y < 0.3f &&
                                pole1.transform.position.y - pole2.transform.position.y > 0.00f &&
                                Mathf.Abs(pole1.transform.position.x - pole2.transform.position.x) < 0.2f &&
                                !pole1.obj && !pole2.obj)
                            {
                                pole1.setVisible(true);
                                pole2.setVisible(true);

                                pole1.name = magent2.gameObject.name;
                                pole2.name = magent1.gameObject.name;
                                ret = true;
                            }
                        }
                    }
                }
            }
    	}
    	return ret;
    }

    private bool checkVerticalPoles(magent1, k1, magent2, k2) {
    	
    	bool ret = false;

    	float r = 0.5f;
    	PoleMono pole1 = magent1.poles[k1];
    	PoleMono pole2 = magent2.poles[k2];

    	if (pole1.pole_type == (int)POLE_TYPE.VERTICAL)
        {
            if (Mathf.Abs(magent1.gameObject.transform.position.x - 
            	magent2.gameObject.transform.position.x) > 2 * r)
            {
                // 垂直
                if (pole1.transform.localPosition.x > 0)
                {
                    if (pole2.transform.localPosition.x < 0)
                    {
                        // Right
                        if (pole1.transform.position.x - pole2.transform.position.x > -0.3f &&
                            pole1.transform.position.x - pole2.transform.position.x < -0.00f &&
                            Mathf.Abs(pole1.transform.position.y - pole2.transform.position.y) < 0.2f &&
                            !pole1.obj && !pole2.obj)
                        {
                            pole1.setVisible(true);
                            pole2.setVisible(true);

                            pole1.name = magent2.gameObject.name;
                            pole2.name = magent1.gameObject.name;
                            ret = true;
                        }
                    }
                }
                else
                {
                    // Left
                    if (pole2.transform.localPosition.x > 0)
                    {
                        if (pole1.transform.position.x - pole2.transform.position.x < 0.3f &&
                            pole1.transform.position.x - pole2.transform.position.x > 0.00f &&
                            Mathf.Abs(pole1.transform.position.y - pole2.transform.position.y) < 0.2f &&
                            !pole1.obj && !pole2.obj)
                        {
                            pole1.setVisible(true);
                            pole2.setVisible(true);

                            pole1.name = magent2.gameObject.name;
                            pole2.name = magent1.gameObject.name;
                            ret = true;
                        }
                    }
                }
            }
        }
        return ret;
    }

    // 切割
    private bool segmentHorizonMagent(magent1, k1, magent2, k2) {
    	
    	bool ret = false;
    	if (magent2.poles[k2].gameObject.name == "c_right" && 
			magent1.poles[k1].gameObject.name == "c_left" || 
			magent2.poles[k2].gameObject.name == "c_left" && 
			magent1.poles[k1].gameObject.name == "c_right") {
			if (magent2.poles[k2].obj == magent1.gameObject ||
				magent1.poles[k1].obj == magent2.gameObject) {

				// 判断交叉
				if (IsSegmentIntersectionWithSegment(
					startPoint,
					endedPoint,
					magent1.gameObject.transform.position,
					magent2.gameObject.transform.position)) {

					if (magent2.poles [k2].gameObject.name == "c_left") {
						// 向下
						magent2.poles [k2].obj.transform.position = new Vector3 (
							magent2.poles [k2].obj.transform.position.x - 0.25f,
							magent2.poles [k2].obj.transform.position.y);
						
						magent1.poles [k1].obj.transform.position = new Vector3 (
							magent1.poles [k1].obj.transform.position.x + 0.25f,
							magent1.poles [k1].obj.transform.position.y);
					} else {
						// 向上
						magent2.poles [k2].obj.transform.position = new Vector3 (
							magent2.poles [k2].obj.transform.position.x + 0.25f,
							magent2.poles [k2].obj.transform.position.y);
						magent1.poles [k1].obj.transform.position = new Vector3 (
							magent1.poles [k1].obj.transform.position.x - 0.25f,
							magent1.poles [k1].obj.transform.position.y);
					}
					magent2.poles [k2].obj = null;
					magent1.poles [k1].obj = null;
					break;
				}
			}
		}

		return ret;
    }

    private bool segmentVerticalMagent(magent1, k1, magent2, k2) {

    	bool ret = false;
    	if (magent2.poles[k2].gameObject.name == "c_top" && 
			magent1.poles[k1].gameObject.name == "c_bom" ||
			magent2.poles[k2].gameObject.name == "c_bom" && 
			magent1.poles[k1].gameObject.name == "c_top") {

			if (magent2.poles [k2].obj == magent1.gameObject ||
				magent1.poles[k1].obj == magent2.gameObject) {

				// 判断交叉
				if (IsSegmentIntersectionWithSegment(
					startPoint,
					endedPoint,
					magent1.gameObject.transform.position,
					magent2.gameObject.transform.position)) {

					// 判断
					if (magent2.poles [k2].gameObject.name == "c_top") {
						// 向下
						magent2.poles [k2].obj.transform.position = new Vector3 (
							magent2.poles [k2].obj.transform.position.x,
							magent2.poles [k2].obj.transform.position.y + 0.25f);

						magent1.poles [k1].obj.transform.position = new Vector3 (
							magent1.poles [k1].obj.transform.position.x,
							magent1.poles [k1].obj.transform.position.y - 0.25f);
					} else {
						// 向上
						magent2.poles [k2].obj.transform.position = new Vector3 (
							magent2.poles [k2].obj.transform.position.x,
							magent2.poles [k2].obj.transform.position.y - 0.25f);

						magent1.poles [k1].obj.transform.position = new Vector3 (
							magent1.poles [k1].obj.transform.position.x,
							magent1.poles [k1].obj.transform.position.y + 0.25f);
					}

					magent2.poles [k2].obj = null;
					magent1.poles [k1].obj = null;
					break;
				}
			}
		}
		return ret;
    }

    private bool checkMagent() {
    	bool ret = false;
    	int magents_count = magents.Count;
    	for (int i = 0; i < magents_count; i++) {
    		for (int j = i + 1; j < magents_count; j++) {
    			MagentMono first_magent = magents_count[i].GetComponent<MagentMono>();
    			MagentMono second_magent = magents_count[j].GetComponent<MagentMono>();
    			for (int k1 = 0; k1 < first_magent.poles.Count; k1++)
                {
                    for (int k2 = 0; k2 < second_magent.poles.Count; k2++)
                    {
                    	ret = checkHorizonPoles(first_magent, k1, second_magent, k2);
                    	ret = checkVerticalPoles(first_magent, k1, second_magent, k2);
                    }
                }
    		}
    	}
    	return ret;
    }

	// 
	void Awake() {

	}

	//
	void Start() {

	}

	void Update() {
		hideAllMagent();
		checkMagent();
		if (Input.GetMouseButtonDown(0)) {

			touched = true;
			use_tm = 0.0f;
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
		if (touched) {
			use_tm = use_tm + Time.deltaTime;
		}
        if (Input.GetMouseButtonUp (0)) {
			touched = false;
            endedPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (use_tm < 0.5f)
            {
                // 切割
                for (int i = 0; i < magents.Count; i++)
                {
                    for (int j = i + 1; j < magents.Count; j++)
                    {
                    	GameObject o1 = magents[i];
                        GameObject o2 = magents[j];
                        MagentMono magent1 = o1.GetComponent<MagentMono>();
                        MagentMono magent2 = o2.GetComponent<MagentMono>();
                        for (int k1 = 0; k1 < magent1.poles.Count; k1++) {
							for (int k2 = 0; k2 < magent2.poles.Count; k2++) {
								// 左右
								segmentHorizonMagent(magent1, k1, magent2, k2);
								// 上下
								segmentVerticalMagent(magent1, k1, magent2, k2);
							}
						}
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 trailPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            trailPosition.z = -9.0f;
            sliding_obj.transform.position = trailPosition;
        }
	}


	//计算两个数字是否接近相等
    public bool IsApproximately(double a, double b)
    {
        return IsApproximately(a, b, 0.01);
    }

    //计算两个数字是否接近相等,阈值是dvalue
    public bool IsApproximately(double a, double b, double dvalue)
    {
        double delta = a - b;
        return delta >= -dvalue && delta <= dvalue;
    }

	// 两直线
	public bool IsSegmentIntersectionWithSegment(Vector3 segment0Start, Vector3 segment0End,
       Vector3 segment1Start, Vector3 segment1End)
    {
        Vector2 p = segment0Start;
        Vector2 r = segment0End - segment0Start;
        Vector2 q = segment1Start;
        Vector2 s = segment1End - segment1Start;
        Vector2 pq = q - p;
        float rxs = r.x * s.y - r.y * s.x;
        float pqxr = pq.x * r.y - pq.y * r.x;
        if (IsApproximately(rxs, 0f))
        {
            if (IsApproximately(pqxr, 0f))
            {
                return true;
            }
            return false;
        }
        float pqxs = pq.x * s.y - pq.y * s.x;
        float t = pqxs / rxs;
        float u = pqxr / rxs;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }

    public GameObject addMagentObject(GameObject o)
    {
        if (o.GetComponent<MagentMono>())
        {
            magents.Add(o);
            int count = o.transform.childCount;
            MagentMono magent = o.GetComponent<MagentMono>();
            for (int i = 0; i < count; i++)
            {
                PoleMono pole = o.transform.GetChild(i).GetComponent<PoleMono>();
                pole.setVisible(false);
                magent.poles.Add(pole);
            }
        }
        return o;
    }

    public void removeManagerObject(GameObject o)
    {
        if (o.GetComponent<MagentMono>())
        {
            MagentMono magent = o.GetComponent<MagentMono>();
            magent.poles.Clear();
            magents.Remove(o);
        }
    }

    public string getObjectInfo(GameObject o)
    {
        string ret = "";
        if (o.GetComponent<MagentMono>())
        {
            int count = o.transform.childCount;
            MagentMono magent = o.GetComponent<MagentMono>();
            for (int i = 0; i < count; i++)
            {
                PoleMono pole = o.transform.GetChild(i).GetComponent<PoleMono>();
                if (pole.name != "")
                {
                    ret = pole.gameObject.name + "&" + pole.name;
                    GameObject dec = GameObject.Find(pole.name);
                    if (dec)
                    {
                        pole.obj = dec;
                        if (pole.gameObject.name == "c_top")
                        {
                            // 顶磁条
                            o.transform.position = new Vector3(dec.transform.position.x, dec.transform.position.y - 1.0f, 0);
                        }
                        else if (pole.gameObject.name == "c_right")
                        {
                            // 右磁条
                            o.transform.position = new Vector3(dec.transform.position.x - 1.0f, dec.transform.position.y, 0);
                        }
                        else if (pole.gameObject.name == "c_left")
                        {
                            // 左磁条
                            o.transform.position = new Vector3(dec.transform.position.x + 1.0f, dec.transform.position.y, 0);
                        }
                        else if (pole.gameObject.name == "c_bom")
                        {
                            // 底磁条
                            o.transform.position = new Vector3(dec.transform.position.x, dec.transform.position.y + 1.0f, 0);
                        }
                    }

                    // dec
                    int count_dec = dec.transform.childCount;
                    MagentMono magent_dec = dec.GetComponent<MagentMono>();
                    for (int j = 0; j < count_dec; j++)
                    {
                        PoleMono pole_dec = dec.transform.GetChild(j).GetComponent<PoleMono>();
                        if (pole_dec.name != "")
                        {
                            pole_dec.obj = o;
                        }
                    }
                }
            }
        }

        return ret;
    }

    public void DragMoved(GameObject o)
    {
        MagentMono magent = o.GetComponent<MagentMono>();
        if (magent.objects != null)
        {
            for (int i = 0; i < magent.objects.Count; i++)
            {
                MagentMono mono = magent.objects[i].GetComponent<MagentMono>();
                mono.gameObject.transform.position = new Vector3(
                    o.transform.position.x + mono.index_x,
                    o.transform.position.y + mono.index_y);
            }
        }
        
    }

    private void ClearList(List<GameObject> list, GameObject o)
    {
        if (o)
        {
            if (!list.Contains(o))
            {
                list.Add(o);
                MagentMono magent = o.GetComponent<MagentMono>();
                magent.index_x = 0;
                magent.index_y = 0;
                int count = o.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    PoleMono pole = o.transform.GetChild(i).GetComponent<PoleMono>();
                    ClearList(list, pole.obj);
                }
            }
        }
    }

    // ClearIndex
    private void ClearIndex(GameObject o)
    {
        MagentMono magent = o.GetComponent<MagentMono>();
		magent.index_x = 0;
		magent.index_y = 0;
        List<GameObject> list = new List<GameObject>();
        int count = o.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            PoleMono pole = o.transform.GetChild(i).GetComponent<PoleMono>();
            if (pole) {
                ClearList(list, pole.obj);
            }
        }
    }

    // 吸磁移动
    private bool PutInList(List<GameObject> list, GameObject o, int dir, int parent_x, int parent_y)
    {
        bool ret = false;
        if (o)
        {
            if (!list.Contains(o))
            {
                list.Add(o);
                MagentMono magent = o.GetComponent<MagentMono>();
                if (dir == 1)
                {
                    // Right
                    magent.index_x = parent_x + 1;
                    magent.index_y = parent_y + 0;
                }
                else if (dir == 2)
                {
                    // Left
                    magent.index_x = parent_x - 1;
                    magent.index_y = parent_y + 0;
                }
                else if (dir == 3)
                {
                    // Top
                    magent.index_x = parent_x + 0;
                    magent.index_y = parent_y + 1;
                }
                else if (dir == 4)
                {
                    // Bottom
                    magent.index_x = parent_x + 0;
                    magent.index_y = parent_y - 1;
                }
                int count = o.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    PoleMono pole = o.transform.GetChild(i).GetComponent<PoleMono>();
                    int dir2 = 0;
                    if ("c_right" == pole.gameObject.name)
                    {
                        dir2 = 1;
                    }
                    else if ("c_left" == pole.gameObject.name)
                    {
                        dir2 = 2;
                    }
                    else if ("c_top" == pole.gameObject.name)
                    {
                        dir2 = 3;
                    }
                    else if ("c_bom" == pole.gameObject.name)
                    {
                        dir2 = 4;
                    }
                    PutInList(list, pole.obj, dir2, magent.index_x, magent.index_y);
                }
            }
        }
        return ret;
    }

    public void CalList(GameObject o)
    {
        ClearIndex(o);
        MagentMono magent = o.GetComponent<MagentMono>();
        magent.index_x = 0;
        magent.index_y = 0;
        List<GameObject> list = new List<GameObject>();
        int count = o.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            PoleMono pole = o.transform.GetChild(i).GetComponent<PoleMono>();
            int dir = 0;
            if ("c_right" == pole.gameObject.name)
            {
                dir = 1;
            }
            else if ("c_left" == pole.gameObject.name)
            {
                dir = 2;
            }
            else if ("c_top" == pole.gameObject.name)
            {
                dir = 3;
            }
            else if ("c_bom" == pole.gameObject.name)
            {
                dir = 4;
            }
            PutInList(list, pole.obj, dir, 0, 0);
        }
        magent.objects = list;
    }
}