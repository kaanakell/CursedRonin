using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrowPoint : MonoBehaviour
{
    public GameObject kunai;
    public float launchForce;
    public Transform throwPoint;


    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    Vector2 direction;
    public bool isAimOpen;
    private PlayerTeleport lastKunai;

    public int ammo = 1;
    public TMP_Text ammoDisplay;

    private bool throwed;
    private bool canRestock = false;

    void Start()
    {
        points = new GameObject[numberOfPoints];
        
      
    }

    void Update()
    {
        ammoDisplay.text = ammo.ToString();
        Vector2 throwPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - throwPosition;
        transform.right = direction;


        Kunai_Throw();

        Kunai_Teleport();

        Kunai_Restock();

        
    }
    private void Kunai_Throw()
    {
        if (throwed)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < numberOfPoints; i++)
                points[i] = Instantiate(point, throwPoint.position, throwPoint.rotation);
        }
        if (Input.GetMouseButton(1))
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            for (int i = 0; i < numberOfPoints; i++)
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            foreach (GameObject points_temp in points)
                Destroy(points_temp);
            Throw();        
        }
    }


    private void Kunai_Teleport()
    {
        if (lastKunai == null)
            return;


        if (Input.GetKeyDown(KeyCode.Q) && lastKunai.hasHit)
        {
            ammo++;
            throwed = false;
            transform.parent.position = lastKunai.kunai_child.position;
            Destroy(lastKunai.gameObject);
        }
    }

    void Throw()
    {
        ammo--;
        throwed = true;
        GameObject newKunai = Instantiate(kunai, throwPoint.position, throwPoint.rotation);
        newKunai.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        lastKunai = newKunai.GetComponent<PlayerTeleport>();
    }

    private void Kunai_Restock()
    {
        if (lastKunai == null)
            return;


        if (Input.GetKeyDown(KeyCode.R))
        {
            ammo++;
            throwed = false;
            Destroy(lastKunai.gameObject);
        }
    }

    Vector3 PointPosition(float t)
    {
        Vector3 position = (Vector2)throwPoint.position + (direction.normalized * launchForce * t);

        return new Vector3(position.x, position.y, 10);
    }


}
