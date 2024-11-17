using Horror.Chores;
using Horror.Chores.UI;
using Horror.InputSystem;
using UnityEngine;

namespace Horror.Player
{
	public class ControlsTutorial : MonoBehaviour
	{
		[SerializeField] private InputReader inputReader;
		[SerializeField] private ChoreProgressor moveChoreProgressor;
		[SerializeField] private ChoreProgressor adjustListChoreProgressor;

		private bool hasMoved = false;


		private void Awake()
		{
			inputReader.ToggleListEvent += ToggleListTutorial;
		}

		private void OnDisable()
		{
			inputReader.ToggleListEvent -= ToggleListTutorial;
		}

		private void Update()
		{
			if (hasMoved || !(inputReader.CurrentMove.sqrMagnitude > 0.1f)) return;

			moveChoreProgressor.ProgressChore();
			hasMoved = true;
		}

		private void ToggleListTutorial()
		{
			adjustListChoreProgressor.ProgressChore();
			inputReader.ToggleListEvent -= ToggleListTutorial;
		}
	}
}