using System.Collections.Generic;

public static class WaveData
{
	public static List<(string sceneName, float delay)> RetrieveWaveData(int waveNumber)
	{
		List<(string sceneName, float delay)> waveData;

		switch (waveNumber)
		{
			case 1:
				waveData = new List<(string sceneName, float delay)>
				{
					("blueTank", 1.5f),
					("blueTank", 1.5f),
					("blueTank", 1.5f),
					("blueTank", 1.5f),
					("greenTank", 1.5f),
					("blueTank", 1.5f),
					("blueTank", 1.5f)
				};
				break;

			case 2:
				waveData = new List<(string sceneName, float delay)>
				{
					("blueTank", 1.3f),
					("blueTank", 1.3f),
					("blueTank", 1.3f),
					("blueTank", 1.3f),
					("blueTank", 1.3f),
					("blueTank", 1.3f),
					("greenTank", 1.3f),
					("redTank", 1.3f),
					("redTank", 1.3f)
				};
				break;

			case 3:
				waveData = new List<(string sceneName, float delay)>
				{
					("greenTank", 1.1f),
					("greenTank", 1.1f),
					("redTank", 1.1f),
					("blueTank", 1.1f),
					("blueTank", 1.1f),
					("blueTank", 1.1f),
					("blueTank", 1.1f),
					("blueTank", 1.1f),
					("blueTank", 1.1f),
					("blueTank", 1.1f),
					("redTank", 1.1f)
				};
				break;

			case 4:
				waveData = new List<(string sceneName, float delay)>
				{
					("blueTank", 2.0f),
					("blueTank", 1.8f),
					("redTank", 1.6f),
					("blueTank", 1.4f),
					("blueTank", 1.2f),
					("blueTank", 1.0f),
					("redTank", 0.9f),
					("greenTank", 0.8f),
					("greenTank", 0.7f),
					("blueTank", 0.65f),
					("blueTank", 0.6f),
					("redTank", 0.55f)
				};
				break;

			case 5:
				waveData = new List<(string sceneName, float delay)>
				{
					("redTank", 0.6f),
					("greenTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("redTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("blueTank", 0.6f),
					("greenTank", 0.6f),
					("greenTank", 0.6f)
				};
				break;

			case 6:
				waveData = new List<(string sceneName, float delay)>
				{
					("redTank", 0.5f),
					("greenTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("redTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("blueTank", 0.5f),
					("greenTank", 0.5f),
					("redTank", 0.5f)
				};
				break;

			case 7:
				waveData = new List<(string sceneName, float delay)>
				{
					("greenTank", 0.45f),
					("redTank", 0.45f),
					("greenTank", 0.45f),
					("redTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("blueTank", 0.45f),
					("greenTank", 0.45f)
				};
				break;

			case 8:
				waveData = new List<(string sceneName, float delay)>
				{
					("redTank", 0.4f),
					("greenTank", 0.4f),
					("redTank", 0.4f),
					("greenTank", 0.4f),
					("redTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("blueTank", 0.4f),
					("greenTank", 0.4f)
				};
				break;

			case 9:
				waveData = new List<(string sceneName, float delay)>
				{
					("greenTank", 0.35f),
					("redTank", 0.35f),
					("greenTank", 0.35f),
					("redTank", 0.35f),
					("greenTank", 0.35f),
					("redTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("blueTank", 0.35f),
					("redTank", 0.35f)
				};
				break;

			case 10:
				waveData = new List<(string sceneName, float delay)>
				{
					("redTank", 0.3f),
					("greenTank", 0.3f),
					("redTank", 0.3f),
					("greenTank", 0.3f),
					("redTank", 0.3f),
					("greenTank", 0.3f),
					("redTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("blueTank", 0.3f),
					("greenTank", 0.3f)
				};
				break;


			default:
				return new List<(string sceneName, float delay)>();
		}

		return waveData;
	}
}