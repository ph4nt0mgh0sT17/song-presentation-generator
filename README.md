# README

## Popis
Tato aplikace je určena k evidenci písní (názvu, textu a zdroje), 
sdílení písní mezi uživately (aplikacemi) a následné generování písniček do prezentace (.pptx formát).

## Návod
1. Jak stáhnout aplikaci

   - Běžte na [Release](https://github.com/ph4nt0mgh0sT17/song-presentation-generator/releases) stránku a stáhněte nejnovější soubor

2. Vytvoření písně

   - V aplikaci stiskněte tlačítko **Vytvořit písničku**
   - Zobrazí se nové okno s možností vyplnit název písničky, Text písničky a také její zdroj
   - Návod jak stylovat text písničky bude v 4. bodě
   - Poté až jsou všechny textové pole vyplněny, máme možnost stisknout tlačítko **Uložit** nebo **Vygenerovat prezentaci**
   - Tlačítko **Uložit** jednoduše uloží písničku v počítačí a zavře dané okno
   - Tlačítko **Vygenerovat prezentaci** otevře nové dialogové okno kde si můžeme vybrat kde uložit danou vygenerovanou prezentaci a po vygenerování se nám otevře
   
3. Vytvoření stylu pro text písničky
   - Veškeré styly jsou uloženy v souboru: **ApplicationConfiguration.json**
   - Každý styl musí mít 5 atributů (Name -> Název (text), FontSize -> velikost písma (číslo), FontFamily -> Jméno fontu (text), FontColor -> Barva fontu (Red, Blue, Green nebo Black) (text), IsBold -> Označení zda styl bude mít tučné písmo (bool hodnota -> true nebo false)
   - Styl tedy bude ve výsledku vypadat takto:
   ```
   {
      "Name": "Můj styl",
      "FontSize": 15,
      "FontFamily": "Arial Black",
      "FontColor": "Black",
      "IsBold":  true
   }
   ```

4. Změna pozadí slidu
   - Změna se provádí stejně jako minulý krok v souboru: **ApplicationConfiguration.json**
   - Stačí změnit položku **SlideBackgroundColor**

5. Stylování textu písně

   - Text písničky bez stylování je zpravidla vygenerován pouze do jednoho slidu
   - Chceme-li nějakou další část písničky vygenerovat na další slide musíme na oddělený řádek mezi částmi písničky 
napsat příkaz **/slide**, který nám zajistí, že se vytvoří nový slide
   - Pokud chceme aplikovat styl (Styly jsou k dispozici v souboru **ApplicationConfiguration.json**), musíme opět na oddělený řádek před částí písničky, na kterou chceme daný styl aplikovat, napsat příkaz **/style(Název stylu)**

6. Evidence písní
   - V evidenci písní naleznem veškeré uložené lokální nebo sdílené (stáhnuté) písničky
   - V této evidenci písní lze písničky sdílet, generovat jejich prezentaci, upravovat či mazat

7. Sdílení písně
   - Píseň lze jednoduše začít sdílet z **Evidence písní** kliknutím na tlačítko **Sdílet** u dané písně
   - Po kliknutí na tlačítko se otevře dialogové okno s textovým polem do kterého vypíšeme identifikátor pod kterým, chceme danou píseň sdílet
   - Poté stačí kliknout na tlačítko **Sdílet píseň** a písnička bude sdílená
   - Jakmile je daná písnička sdílená, můžou si ji ostatní uživatelé v aplikaci stáhnout na svůj počítač a případně upravovat
   - Ke stáhnutí sdílené písničky potřebují uživatelé pouze ID vaší sdílené písně
