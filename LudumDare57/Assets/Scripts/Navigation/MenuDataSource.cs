using UnityEngine;

[CreateAssetMenu(fileName = "MenuSource", menuName = "DataSource/MenuSource")]
public class MenuDataSource : ScriptableObject
{
    public MenuController DataInstance { get; set; }
    [SerializeField] private string _menuId;
    public string menuId { get { return _menuId; } }
}
