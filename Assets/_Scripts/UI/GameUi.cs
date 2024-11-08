using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class GameUi : MonoBehaviour
    {
        public Button playButton;

        public TextMeshProUGUI totalThrowsText;
        public TextMeshProUGUI lastThrowSumText;
        public TextMeshProUGUI lastThrowAvgText;
        public TextMeshProUGUI totalSumText;
        public TextMeshProUGUI totalAvgText;

        public Slider maxValueSlider;
        public TextMeshProUGUI sliderValueText;

        public void InitSlider()
        {
            maxValueSlider.maxValue = 100;
            maxValueSlider.minValue = 1;
            maxValueSlider.value = 2;
            maxValueSlider.wholeNumbers = true;
            UpdateSliderValue(maxValueSlider.value);

            maxValueSlider.onValueChanged.AddListener(UpdateSliderValue);
        }

        public void UpdateTotalThrows(int totalThrows)
        {
            totalThrowsText.text = $"Бросков: {totalThrows.ToString()}";
        }

        public void UpdateLastThrowSum(int lastThrowSum)
        {
            lastThrowSumText.text = $"Сумма последнего броска: {lastThrowSum.ToString()}";
        }

        public void UpdateLastThrowAvg(float lastThrowAvg)
        {
            lastThrowAvgText.text = $"Среднее последнего броска: {lastThrowAvg:F2}";
        }

        public void UpdateTotalSum(int totalSum)
        {
            totalSumText.text = $"Сумма всех бросков: {totalSum.ToString()}";
        }

        public void UpdateTotalAvg(float totalAvg)
        {
            totalAvgText.text = $"Общее среднее: {totalAvg:F2}";
        }

        private void UpdateSliderValue(float value) =>
            sliderValueText.text = value.ToString("F0", CultureInfo.InvariantCulture);
    }
}