using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//tạo tham số cho tài nguyên

[CreateAssetMenu(fileName = "NewTaiNguyen", menuName = "ScriptableObjects/TaiNguyen")]
public class TaiNguyen : ScriptableObject
{
    //các chỉ số tài nguyên
    public int Gold = 0;
    public int MaxEnergy ;
    public int Energy ;  
    public int Star = 0;
}
