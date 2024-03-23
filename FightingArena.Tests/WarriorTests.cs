namespace FightingArena.Tests
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class WarriorTests
    {



        [Test]
        public void TestIfConstructorWorksCorrectly() 
        {
            Warrior warrior = new Warrior("Pesho", 55, 55);
            string expectedName = "Pesho";
            int expectedDamage = 55;
            int expectedHealth = 55;

            FieldInfo nameField = this.GetField("name");
            FieldInfo damageField = this.GetField("damage");
            FieldInfo healthField = this.GetField("hp");



            string actualName = (string)nameField.GetValue(warrior);
            int actualDamage = (int)damageField.GetValue(warrior);
            int actualHealth = (int)healthField.GetValue(warrior);

            Assert.AreEqual(expectedName, actualName, "Cosntructor should intialize the name of the Warrior!");
            Assert.AreEqual(expectedDamage, actualDamage, "Cosntructor should intialize the damage of the Warrior!");
            Assert.AreEqual(expectedHealth, actualHealth, "Cosntructor should intialize the HP of the Warrior!");
        }

        private FieldInfo GetField(string fieldName) => typeof(Warrior)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic|BindingFlags.Static).
            First(n=>n.Name == fieldName);

        [Test]
        public void TestNameGetter() 
        {
            string expectedName = "Pesho";
            Warrior warrior = new Warrior(expectedName, 55, 55);
            string actualName = warrior.Name;
            Assert.AreEqual(expectedName, actualName,
                "Getter of the property Name should return the value of the name!");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("                 ")]
        public void TestNameSetterValidation(string name) 
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Warrior warrior = new Warrior(name, 85, 85);
            }, "Name should not be empty or whitespace!");
        }

        public void TestDamageGetter() 
        {
            int expectedDamage = 55;
            Warrior warrior = new Warrior("Pesho", expectedDamage, 55);
            int actualDamage = warrior.Damage;
            Assert.AreEqual(expectedDamage, actualDamage,
                "Getter of the property Damage should return the value of the damage!");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TestDamageSetterValidation(int damage)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Warrior warrior = new Warrior("Pesho", damage, 85);
            }, "Damage value should be positive!");
        }


        public void TestHpGetter()
        {
            int expectedHp = 55;
            Warrior warrior = new Warrior("Pesho", 55, expectedHp);
            int actualHp = warrior.HP;
            Assert.AreEqual(expectedHp, actualHp,
                "Getter of the property HP should return the value of the hp!");
        }

        [TestCase(-1)]
        public void TestHpSetterValidation(int hp)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Warrior warrior = new Warrior("Pesho", 55, hp);
            }, "HP should not be negative!");
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        public void AttackShouldThrowErrorWhenAttackingWarriorHpIsTooLow(int startHp) 
        {
            Warrior a = new Warrior("Pesho", 55, startHp);
            Warrior b = new Warrior("Gosho", 55, 100);

            Assert.Throws<InvalidOperationException>(() =>
            {
                a.Attack(b);
            }, "Your HP is too low in order to attack other warriors!");
        }
        [TestCase(0)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(30)]
        public void AttackShouldThrowErrorWhenDefendingWarriorHpIsTooLow(int startHp)
        {
           
            Warrior a = new Warrior("Pesho", 55, 100);
            Warrior b = new Warrior("Gosho", 55, startHp);

            Assert.Throws<InvalidOperationException>(() =>
            {
                a.Attack(b);
            }, $"Enemy HP must be greater than {this.GetField("MIN_ATTACK_HP")} in order to attack him!");
      
        }
        [TestCase(50,60)]
        [TestCase(50,51)]
        public void AttackShouldThrowErrorWhenDefendingWarriorIsStronger(int attackerHp, int defenderDamage)
        {
            Warrior a = new Warrior("Pesho", 50, attackerHp);
            Warrior b = new Warrior("Gosho", defenderDamage, 50);

            Assert.Throws<InvalidOperationException>(() =>
            {
                a.Attack(b);
            }, "You are trying to attack too strong enemy");
        }
        public void SuccessAttackShouldDecreaseAttackerHp(int attackerHp, int defenderDamage) 
        {
            Warrior a = new Warrior("Pesho", 50, attackerHp);
            Warrior b = new Warrior("Gosho", defenderDamage, 55); 
        }
    }
}
