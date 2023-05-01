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
		if(LineLength > 0) {
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
	}
	public int LineLength { get { return line.Count; } }
}

public class CustomerManager : MonoBehaviour
{
	public Sprite[] variants;
	public Customer customerPrefab;
	public Transform customerParent;
	public CustomerLine[] lines;
	public float startPause = 1.5f;
	public Vector2 spawnInterval;

	private bool isPlaying;
	private int maxCustomers = 0;
	private int currentCustomers = 0;
	private Coroutine customerRoutine;

	private void Start() {
		foreach(CustomerLine line in lines) {
			maxCustomers += line.maxCustomers;
		}
		StartCoroutine(StartSpawnCustomers());
	}

	private void OnEnable() {
		Events.CustomerLeft += CustomerLeft;
		Events.EndGame += EndGame;
		isPlaying = true;
	}

	private void OnDisable() {
		EndGame();
	}

	private void EndGame() {
		Events.CustomerLeft -= CustomerLeft;
		Events.EndGame -= EndGame;
		isPlaying = false;
		if(customerRoutine != null) {
			StopCoroutine(customerRoutine);
		}
	}

	private IEnumerator StartSpawnCustomers() {
		yield return new WaitForSeconds(startPause);
		customerRoutine = StartCoroutine(DoSpawnCustomer());
	}

	private IEnumerator DoSpawnCustomer() {
		while(isPlaying && currentCustomers < maxCustomers) {
			IEnumerable<CustomerLine> openLines = lines.Where(x => x.LineLength < x.maxCustomers);
			if(openLines.Count() > 0) {
				//Randomly pick from the shortest lines
				openLines = openLines.OrderBy(x => x.LineLength);
				int customers = openLines.ElementAt(0).LineLength;
				openLines = openLines.Where(x => x.LineLength == customers);
				int i = Random.Range(0, openLines.Count());
				Customer c = Instantiate(customerPrefab, customerParent);
				c.SetSprite(variants[Random.Range(0, variants.Length)]);
				openLines.ElementAt(i).AddCustomer(c);
				currentCustomers++;
			}
			if(currentCustomers < maxCustomers) {
				yield return new WaitForSeconds(Random.Range(spawnInterval.x, spawnInterval.y));
			}
		}
	}

	private void CustomerLeft() {
		currentCustomers--;
		StartCoroutine(DoCustomerLeft());
	}

	private IEnumerator DoCustomerLeft() {
		yield return null;
		foreach(CustomerLine l in lines) {
			l.RemoveCustomer();
		}
		if(currentCustomers == maxCustomers - 1) {
			yield return new WaitForSeconds(Random.Range(spawnInterval.x, spawnInterval.y));
			customerRoutine = StartCoroutine(DoSpawnCustomer());
		}
	}
}
