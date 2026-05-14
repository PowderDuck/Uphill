using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Uphill.Scripts.Sections;

namespace Uphill.Scripts.Managers
{
    public class GenerationManager : MonoBehaviour
    {
        [SerializeField] private List<SectionBase> _sections = default!;
        [SerializeField] private Vector2 _gridSize = new(5, 5);
        [SerializeField] private Vector2 _reachGridSize = new(3, 3);
        [SerializeField] private Transform _gridHolder = default!;

        [SerializeField] private Climber _climber = default!;

        private Vector2 _currentGridPosition = Vector2.zero;
        private Vector2 _previousGridPosition { get; set; } = -Vector3.up;

        private Dictionary<Vector2, SectionBase> _sectionGrid { get; } = new();
        private List<SectionBase> _activeSections { get; } = new();

        private void Start() => _sections.Sort((a, b) =>
            a.OperatingHeight.CompareTo(b.OperatingHeight));

        private void FixedUpdate()
        {
            _currentGridPosition.Set(
                Mathf.Floor((_climber.transform.position.x + (_gridSize.x / 2f)) / _gridSize.x),
                Mathf.Floor((_climber.transform.position.y + (_gridSize.y / 2f)) / _gridSize.y));

            if (_previousGridPosition != _currentGridPosition)
            {
                ManageGrid();
                _previousGridPosition = _currentGridPosition;
            }

            foreach (var sectionSection in _activeSections)
            {
                sectionSection.Update();
            }
        }

        private void ManageGrid()
        {
            _activeSections.Clear();

            for (var y = 0; y < _reachGridSize.y; y++)
            {
                for (var x = 0; x < _reachGridSize.x; x++)
                {
                    var gridPosition = GetGridPosition(x, y) + _previousGridPosition;

                    if (_sectionGrid.TryGetValue(
                        gridPosition, out var previousSectionSection))
                    {
                        previousSectionSection.Disable();
                        previousSectionSection.gameObject.SetActive(false);
                    }
                }
            }

            for (var y = 0; y < _reachGridSize.y; y++)
            {
                for (var x = 0; x < _reachGridSize.x; x++)
                {
                    var gridPosition = GetGridPosition(x, y) + _currentGridPosition;

                    if (_sectionGrid.TryGetValue(
                        gridPosition, out var sectionSection))
                    {
                        sectionSection.gameObject.SetActive(true);
                        sectionSection.Enable();

                        _activeSections.Add(sectionSection);
                    }
                    else
                    {
                        _sectionGrid.Add(
                            gridPosition, CreateSection(gridPosition));
                    }
                }
            }
        }

        private Vector2 GetGridPosition(float x, float y)
        {
            return new Vector2(
                (-(int)_reachGridSize.x / 2) + x,
                (-(int)_reachGridSize.y / 2) + y);
        }

        private SectionBase CreateSection(Vector2 gridPosition)
        {
            var sectionPosition = new Vector2(
                gridPosition.x * _gridSize.x,
                gridPosition.y * _gridSize.y);

            var section = Instantiate(
                GetNextSection(sectionPosition.y), sectionPosition, Quaternion.identity);

            section.transform.SetParent(_gridHolder);
            section.Initialize();

            return section;
        }

        private SectionBase GetNextSection(float height)
        {
            if (_sections.Count <= 1)
            {
                return _sections.FirstOrDefault();
            }

            for (var i = 0; i < _sections.Count - 1; i++)
            {
                if (_sections[i].OperatingHeight <= height
                    && _sections[i + 1].OperatingHeight > height)
                {
                    return _sections[i];
                }
            }

            return _sections.LastOrDefault();
        }
    }
}
