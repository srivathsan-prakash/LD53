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
		}
	}
	public void RemoveCustomer() {
		line.Dequeue();
		int i = 0;
		foreach(Customer c in line) {
			c.transform.position = head.position + Vector3.right * spacing * i;
			i++;
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
	public float lineSpacing;
	public int maxPerLine;
	public float spawnInterval;

	private void Start() {
		InvokeRepeating(nameof(SpawnNewCustomer), 1, spawnInterval);
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

	//When spawning a new customer, check for the first open line and add it there
	//Line will need to be informed when a customer leaves for any reason
	//and then call dequeue - maybe we fire a CustomerLeft event and check Peek for null?
	//If customers in the middle of the line can leave, we won't be able to use a queue at all
}
