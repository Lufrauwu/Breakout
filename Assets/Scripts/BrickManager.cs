using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private int _maxRows = 5;
    private int _maxCols = 12;
    private GameObject _brickContainer;
    private float _shiftAmount = 1f;
    [SerializeField] private float _initialSpawnX = -1.96f;
    [SerializeField] private float _initialSpawnY = 3.325f;
    public static BrickManager _instance;
    public Brick _brickPrefab;
    public static BrickManager Instance => _instance;
    public Sprite[] _sprites;
    public Color[] _brickColor;
    public List<Brick> _remainingBricks { get; set; }
    public int _initialBricksCount { get; set; }
    public List<int[,]> LevelData { get; set; }
    public int _currentLevel = default;
    public static event Action OnLevelLoad;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        this.LevelData = this.LoadLevelData();
        this._brickContainer = new GameObject("BricksContainer");
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        this._remainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelData[this._currentLevel];
        float currentSpawnX = _initialSpawnX;
        float currentSpawnY = _initialSpawnY;
        float zShift = 0;

        for (int row = 0; row < this._maxRows; row++)
        {
            for (int col = 0; col < this._maxCols; col++)
            {
                int brickType = currentLevelData[row, col];
                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(_brickPrefab, new Vector3(currentSpawnX,currentSpawnY, 0.0f - zShift), Quaternion.identity)as Brick;
                    newBrick.Init(_brickContainer.transform, this._sprites[brickType - 1], this._brickColor[brickType], brickType);
                    this._remainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += _shiftAmount;
                if (col + 1 == this._maxCols)
                {
                    currentSpawnX = _initialSpawnX;
                }
            }

            currentSpawnY -= _shiftAmount;
        }

        this._initialBricksCount = this._remainingBricks.Count;
        OnLevelLoad?.Invoke();
    }

    private List<int[,]> LoadLevelData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        string [] rows = text.text.Split (new char [] {}, StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> leveldata = new List<int[,]>();
        int[,] currentLevel = new int[_maxRows, _maxCols];
        int currentRow = 0;
        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];
            if (line.IndexOf("--")== -1)
            {
                string[] bricks = line.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                leveldata.Add(currentLevel);
                currentLevel = new int[_maxRows, _maxCols];
            }
        }

        return leveldata;
    }

    public void LoadLevel(int level)
    {
        this._currentLevel = level;
        this.ClearRemainingBricks();
        this.GenerateBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this._remainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    public void LoadNextLevel()
    {
        this._currentLevel++;
        if (this._currentLevel >= this.LevelData.Count)
        {
            GameManager.Instance.ShowVictory();
        }
        else
        {
            this.LoadLevel(this._currentLevel);
        }
    }
}
