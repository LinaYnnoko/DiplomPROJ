using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebImport : MonoBehaviour
{
    Material mainTexture; // ссылка на текстуру экрана

    private string[] webImages =
   {
   "https://images.wallpaperscraft.ru/image/single/les_chelovek_odinochestvo_116306_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/domik_zima_sneg_134709_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/les_tropinka_leto_125991_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/les_tuman_derevia_126479_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/zvezdnoe_nebo_lodka_otrazhenie_125803_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/pejzazh_gory_solntse_140434_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/lodka_gory_ozero_135258_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/skamejka_osen_park_125807_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/vodopad_obryv_kamennyj_141850_1920x1080.jpg",
   "https://images.wallpaperscraft.ru/image/single/osen_les_park_128379_1920x1080.jpg"
    };

    private Texture[] Images = new Texture[10]; // массив из загруженных изображений
    private int i = 0; // счетчик, чтобы знать какое изображение показывается

    private void Start()
    {
        mainTexture = GetComponent<Renderer>().material; // инициализируем ссылку на текстуру
        StartCoroutine(ShowImages()); // запускаем корутину, сменяющую изображения
    }

    private IEnumerator ShowImages() // корутина смены изображений
    {
        while (true)
        {
            if (Images[i] == null) // если требуемой текстуры нет в массиве
            {
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(webImages[i]); // загружаем изображение по ссылке      
                yield return www.SendWebRequest(); // ждем когда изображение загрузится
                Images[i] = ((DownloadHandlerTexture)www.downloadHandler).texture; ; // записываем загруженную текстуру в массив
            }
            mainTexture.mainTexture = Images[i]; // устанавливаем текстуру из массива изображений
            i++; // увеличиваем счетчик
            if (i == 10)
            {
                i = 0; // если загрузили уже 9, возвращаемся к первому
            }
            yield return new WaitForSeconds(3f); // ждем 3 секунды между сменой изображений
        }
    }


}
