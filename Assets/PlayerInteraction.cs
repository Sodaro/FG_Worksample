using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    //GameObject weaponObject;
    Weapon weapon;
    [SerializeField] Transform weaponPos;
    [SerializeField] TMP_Text bulletText;
    [SerializeField] GameObject bulletUI;
    [SerializeField] GameObject startingWeapon;
	[SerializeField] GameObject mouseTarget;

    [SerializeField] Texture2D crosshair;
    float defaultLookRange = 2f;

    Transform target;
    //bool isZoomedIn = false;

	private void Awake()
	{
		if (startingWeapon)
		{
            GameObject weaponObj = Instantiate(startingWeapon);
            weapon = weaponObj.GetComponentInChildren<Weapon>();
            PickUpWeapon(weapon);
        }
	}

	// Start is called before the first frame update
	void Start()
    {
        Vector2 offset = new Vector2(crosshair.width / 2, crosshair.height / 2);
        Cursor.SetCursor(crosshair, offset, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
        if (Input.GetKeyDown(KeyCode.F1))
            Debug.Break();
        if (Input.GetKeyDown(KeyCode.R))
		{
            if (weapon)
			{
                weapon.Reload();
                bulletText.text = weapon.Ammo.ToString();
            }
		}
        if (Input.GetMouseButton(0))
		{
            if (weapon)
            {
                weapon.Fire();
                bulletText.text = weapon.Ammo.ToString();
            }
            //Debug.Log(weapon);
        }
        if (Input.GetMouseButtonUp(0))
		{
            if (weapon)
            {
                weapon.Stop();
            }
        }
        if (weapon)
		{
            if (Input.GetMouseButtonDown(1))
			{
                weapon.Throw();
                bulletUI.SetActive(false);
                weapon = null;
			}
		}
        if (!weapon)
		{
            if (Input.GetKeyDown(KeyCode.E))
			{
                AttemptWeaponPickup();
            }
		}
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMask))
        {
            Vector3 targetPoint = hit.point;
            targetPoint.y = weaponPos.position.y;
            if (Input.GetKey(KeyCode.Q))
            {
                if (weapon)
                {
                    if (Vector3.Distance(targetPoint, transform.position) < weapon.Data.Range)
                            mouseTarget.transform.position = targetPoint;
                    else
                        mouseTarget.transform.position = weaponPos.position + (targetPoint - weaponPos.position).normalized * weapon.Data.Range;
                }
                else
                {
                    if (Vector3.Distance(targetPoint, transform.position) < defaultLookRange)
                        mouseTarget.transform.position = targetPoint;
                    else
                        mouseTarget.transform.position = transform.position + (targetPoint - transform.position).normalized * defaultLookRange;
                }

                Vector3 dir = (mouseTarget.transform.position - transform.position).normalized;

                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
                Vector3 weaponDir = (targetPoint - weaponPos.position).normalized;

                weaponPos.rotation = Quaternion.LookRotation(weaponDir);
            }
            else
			{
                mouseTarget.transform.position = transform.position;

                Vector3 dir = (targetPoint - transform.position).normalized;

                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
                Vector3 weaponDir = (targetPoint - weaponPos.position).normalized;

                weaponPos.rotation = Quaternion.LookRotation(weaponDir);
            }

        }
    }

    public void AttemptWeaponPickup()
	{
        if (weapon)
            return;
        int layermask = 1 << 7;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, layermask, QueryTriggerInteraction.Collide);
        foreach (var collider in colliders)
        {
            Weapon script;
            if ((script = collider.GetComponent<Weapon>()) != null)
            {
                if (!script.isHeld)
                {
                    weapon = script;
                    PickUpWeapon(weapon);
                    break;
                }
            }
        }
    }
    void PickUpWeapon(Weapon weaponscript)
	{
        weaponscript.PickUp(weaponPos);
        bulletText.text = weaponscript.Ammo.ToString();
        bulletUI.SetActive(true);
    }
}
