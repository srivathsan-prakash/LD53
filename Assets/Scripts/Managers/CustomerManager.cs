using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CustomerLine
{
	public Transform head;
	public int maxCustomers;
	public float spacing;

	private Queue<Customer> line = new Queue<Customer>();

	public void AddCustomer(Customer c) {
		if(LineLength < maxCustomers) {
			line.Enqueue(c);
			c.transform.position = head.position + Vector3.right * spacing * (LineLength - 1);
			if(LineLength == 1) {
				c.SetAsFront();
			}
		}
	}
	public void RemoveCustomer() {
		if(line.Peek() == null) {
			line.Dequeue();
			int i = 0;
			foreach(Customer c in line) {
				c.transform.position = head.position + Vector3.right * spacing * i;
				i++;
			}
			if(line.Count > 0) {
				line.Peek().SetAsFront();
			}
		}
	}
	public int LineLength { get { return line.Count; } }
}

public class CustomerManager : MonoBehaviour
{
	public Sprite[] variants;
	public Customer customerPrefab;
	public Transform customerParent;
	public CustomerLine[] lines;
	public float spawnInterval;

	private void Start() {
		InvokeRepeating(nameof(SpawnNewCustomer), 1, spawnInterval);
	}

	private void OnEnable() {
		Events.CustomerLeft += CustomerLeft;
	}

	private void OnDisable() {
		Events.CustomerLeft -= CustomerLeft;
	}

	private void SpawnNewCustomer() {
		IEnumerable<CustomerLine> openLines = lines.Where(x => x.LineLength < x.maxCustomers);
		if(openLines.Count() > 0) {
			//Randomly pick from the shortest lines
			openLines = openLines.OrderBy(x => x.LineLength);
			int customers = openLines.ElementAt(0).LineLength;
			openLines = openLines.Where(x => x.LineLength == customers);
			int i = Random.Range(0, openLines.Count());
			openLines.ElementAt(i).AddCustomer(Instantiate(customerPrefab, customerParent));
		}
	}

	private void CustomerLeft() {
		StartCoroutine(DoCustomerLeft());
	}

	private IEnumerator DoCustomerLeft() {
		yield return null;
		foreach(CustomerLine l in lines) {
			l.RemoveCustomer();
		}
	}
}
