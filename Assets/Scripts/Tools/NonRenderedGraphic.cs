using UnityEngine.UI;
 
/// <summary>
/// Компонент для имитации прозрачного Image для отлова нажатий в GraphicCaster'е, но при этом не рендерит всю эту область.
/// </summary>
 public class NonRenderedGraphic : Graphic {
	public override void SetMaterialDirty() {
		return;
	}

	public override void SetVerticesDirty() {
		return;
	}

	protected override void OnPopulateMesh(VertexHelper vh) {
		vh.Clear();
		return;
	}
 }