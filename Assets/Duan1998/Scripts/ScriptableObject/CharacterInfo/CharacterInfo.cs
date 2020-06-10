using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
	[CreateAssetMenu(menuName = "ScriptableObject/CharacterInfo")]
	public class CharacterInfo : ScriptableObject {

		[Tooltip("编辑器内对话框颜色")]
		public Color m_color;
		[Tooltip("角色名")]
		public string m_name;
		[Tooltip("明亮立绘")]
		public Sprite m_lightCharacterSprite;
		[Tooltip("灰暗立绘")]
		public Sprite m_greyCharacterSprite;
		[HideInInspector]
		public Sprite m_curCharacterSprite;


		public void SwitchLightSprite()
		{
			m_curCharacterSprite = m_lightCharacterSprite;
		}
		public void SwitchGreySprite()
		{
			m_curCharacterSprite = m_greyCharacterSprite;
		}

	}

	
}