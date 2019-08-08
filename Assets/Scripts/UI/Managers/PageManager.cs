
using System;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class PageManager : MonoBehaviour
{
   [SerializeField] private List<BasePage> _pagesStorage = new List<BasePage>();
   
   private readonly Dictionary<Type, BasePage> Pages = new Dictionary<Type, BasePage>();

   private BasePage _openedPage;

   private static PageManager _instance;
   
   private void Awake()
   {
      _instance = this;

      InitOthers();

      for (var i = 0; i < _pagesStorage.Count; i++)
      {
         var page = _pagesStorage[i].GetComponent<BasePage>();
         page.Init();
         
         Pages.Add(page.GetType(), page);

         if(_openedPage != null)
            continue;
         
         if (page is IStartPage)
            Open(page);
      }
   }

   private void InitOthers()
   {
      PoolObjects.Instance.Init();
   }

   private void Update()
   {
      if (Random.value < 0.5)
      {
         var playe = PoolObjects.Get<Player>();
         playe.transform.position = new Vector3(Random.Range(1f,2f),Random.Range(1f,2f),Random.Range(1f,2f));
      }
      else
      {
         var enemy = PoolObjects.Get<Enemy>();
         enemy.transform.position = new Vector3(Random.Range(1f,2f),Random.Range(1f,2f),Random.Range(1f,2f));
      }
   }

   public static void Open<T>() where T: BasePage => _instance.OpenPage<T>();

   private void OpenPage<T>() where T : BasePage
   {
      ClosePage(_openedPage);

      var pageType = typeof(T);
      if (!Pages.ContainsKey(pageType))
      {
         Debug.LogError("Page not found");
         return;
      }

      Open(Pages[pageType]);
   }

   private void Open(BasePage page)
   {
      _openedPage = page;
      _openedPage.PageState = PageState.Open;
      _openedPage.Open(); 
   }

   private void ClosePage(BasePage closePage)
   {
      if (closePage == null)
         return;

      closePage.PageState = PageState.Close;
      closePage.Close();
   }
}