using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ColorManager : MonoBehaviour
{
	[Inject]
	private GameManager gameManager;
	
	[Inject]
	private PieceController pieceController;
	
	
	[SerializeField] private ColorData colorData;
	[SerializeField] private MeshRenderer referenceMesh;
	[SerializeField] private List<MeshRenderer> pivots;

	private List<Color> _listColor;

	private int colorCount = 0;

	#region Variables

	private List<Color> pool;
	private int length;
	private int index;
	private Color currentColor;
	private Color baseColor;
	private Color target;
	private float normalized;
	private Color targetColor;
	private int currentScore;
	private Color color;

	#endregion
	private void Start()
	{
		_listColor = new List<Color>();

		pool = new List<Color>(colorData.colors);
		length = pool.Count;
		for (int i = 0; i < length; i++)
		{
			 index = Random.Range(0, pool.Count);
			currentColor = pool[index];
			pool.RemoveAt(index);
			_listColor.Add(currentColor);
		}

		SetColor();
		referenceMesh.material.color = _listColor[0];
	}

	private void SetColor()
	{
		//var baseColor = _listColor[Random.Range(1, _listColor.Count)]; Random pivot rengi
		baseColor = _listColor[0];

		target = _listColor[0];
		
		for (int i = 0; i < pivots.Count; i++)
		{
			normalized = (float)(i + 1) / pivots.Count;
			pivots[i].material.color = Color.Lerp(target, baseColor, normalized);
		}
		
	}


	public Color GetColor(int score)
	{
		index = score / colorData.scoreLimit;

		baseColor = _listColor[index];
		targetColor = _listColor[index + 1];

		currentScore = score % colorData.scoreLimit;
		return Color.Lerp(baseColor, targetColor, (float)currentScore / colorData.scoreLimit);
	}
	
	public void UpdateColor(Transform prefab)
	{
		prefab.GetComponent<MeshRenderer>().material.color = referenceMesh.material.color;
		
	}
	
	public void SetNewColor()
	{
		colorCount++;
		color = GetColor(colorCount);
		referenceMesh.material.color = color;
		
	}
	
	
	
}