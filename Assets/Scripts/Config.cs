using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config {

	public const int TESOURO_BASE = 10;
	public const int SOMA_TESOURO = 5;

	public struct Point
	{
		public int x;
		public int y;

		public Point(int _x, int _y){
			x = _x;
			y = _y;
		}

	}


	//INDICES EM Area.vizinhoDificuldade
	public const int DIFICULDADE_MAIORES = 0;
	public const int DIFICULDADE_MENORES = 1;
	public const int DIFICULDADE_IGUAIS = 2;
	public const int DIFICULDADE_MEDIA = 3;
	public const int DIFICULDADE_MINIMA = 4;
	public const int DIFICULDADE_MATOALTO = 5;
	public const int DIFICULDADE_SEPARADA1 = 6;
	public const int DIFICULDADE_SEPARADA2 = 7;
	public const int DIFICULDADE_FINAL = 8;
	public const int DIFICULDADE_BOSS = 9;

}
