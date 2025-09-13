using System.Collections.Generic;
using UnityEngine;
using Uphill.Scripts.Sections;

namespace Uphill.Scripts.Managers
{
    public class GenerationManager : MonoBehaviour
    {
        [SerializeField] private List<SectionController> _sections = default!;
        [SerializeField] private Vector2 _gridSize = new(5, 5);
        [SerializeField] private Vector2 _reachGridSize = new(3, 3);
        [SerializeField] private Transform _gridHolder = default!;

        [SerializeField] private Climber _climber = default!;

        private Vector2 _currentGridPosition = Vector2.zero;
        private Vector2 _previousGridPosition { get; set; } = Vector2.zero;

        private Dictionary<Vector2, SectionController> _sectionGrid { get; } = new();
        private List<SectionController> _activeControllers { get; } = new();

        private void FixedUpdate()
        {
            _currentGridPosition.Set(
                Mathf.Floor(_climber.transform.position.x / _gridSize.x),
                Mathf.Floor(_climber.transform.position.y / _gridSize.y));

            if (_previousGridPosition != _currentGridPosition)
            {
                ManageGrid();
                _previousGridPosition = _currentGridPosition;
            }

            foreach (var sectionController in _activeControllers)
            {
                sectionController.Section.Update();
            }
        }

        private void ManageGrid()
        {
            _activeControllers.Clear();

            for (var x = 0; x < _reachGridSize.x; x++) // TODO: Swap x with y
            {
                for (var y1 = 0; y1 < _reachGridSize.y; y1++)
                {
                    var gridPosition = new Vector2(
                        (-(int)_reachGridSize.x / 2) + x, (-(int)_reachGridSize.y / 2) + y1);
                    if (_sectionGrid.TryGetValue(
                        gridPosition + _previousGridPosition, out var previousSectionController))
                    {
                        previousSectionController.Section.Disable();
                        previousSectionController.gameObject.SetActive(false);
                    }
                }

                for (var y = 0; y < _reachGridSize.y; y++)
                {
                    var gridPosition = new Vector2(
                        (-(int)_reachGridSize.x / 2) + x, (-(int)_reachGridSize.y / 2) + y);
                    // if (_sectionGrid.TryGetValue(
                    //     gridPosition + _previousGridPosition, out var previousSectionController))
                    // {
                    //     previousSectionController.Section.Disable();
                    //     previousSectionController.gameObject.SetActive(false);
                    // }

                    if (_sectionGrid.TryGetValue(
                        gridPosition + _currentGridPosition, out var sectionController))
                    {
                        sectionController.gameObject.SetActive(true);
                        sectionController.Section.Enable();

                        _activeControllers.Add(sectionController);
                    }
                    else
                    {
                        _sectionGrid.Add(
                            gridPosition + _currentGridPosition,
                            CreateSection(_sections[0], gridPosition));
                    }
                }
            }
        }

        private SectionController CreateSection(
            SectionController prefab, Vector2 gridPosition)
        {
            var sectionController = Instantiate(
                prefab,
                new(gridPosition.x * _gridSize.x, gridPosition.y * _gridSize.y),
                Quaternion.identity);
            sectionController.transform.SetParent(_gridHolder);
            sectionController.Section.Initialize();

            return sectionController;
        }
    }
}
