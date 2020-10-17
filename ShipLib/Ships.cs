using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShipLib
{
    public interface IShip : IDisposable
    {
        /// <summary>
        /// if ship only one
        /// </summary>
        /// <param name="ship"></param>
        public abstract void Attack(BaseShip ship);
    }
    public enum Team
    {
        Red,
        Blue
    }

    public abstract class BaseShip : IShip
    {
        protected int _hitPoints;
        protected int _damageMin;
        protected int _damageMax;
        protected string _resultLastAttack;
        protected int _CountDecked;
        private bool disposedValue;
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public int HitPoints { get => _hitPoints; private set => _hitPoints = value; }
        public Team Team;

        public string ResultLastAttack
        {
            get
            {
                if (String.IsNullOrEmpty(_resultLastAttack)) return null;

                return _resultLastAttack;
            }
            private set
            {
                _resultLastAttack = value;
            }
        }

        public BaseShip(int HitPoints, int DamageMin, int DamageMax, Team Team)
        {
            if (DamageMin > DamageMax)
                throw new ArgumentException("Минимальный урон не должен быть меньше максимального");
            _hitPoints = HitPoints;
            _damageMin = DamageMin;
            _damageMax = DamageMax;
            this.Team = Team;
        }
        /// <summary>
        /// Attacking other ship and returns degree of defeat
        /// </summary>
        /// <param name="shipToAttack"></param>
        /// <returns>Degree of defeat as double view of percents</returns>
        public void Attack(BaseShip shipToAttack)
        {
            //checking
            if (shipToAttack.HitPoints <= 0)
                throw new InvalidOperationException("Аттаковать данный корабль не возможно: он мёртв");
            if (shipToAttack.Team == this.Team)
                throw new InvalidOperationException("Атаковать данный корабль не возможно: он союзник");
            if (disposedValue == true)
                throw new ObjectDisposedException("Вызванный объект не может быть вызван, так как вскоре будет очищен.");

            //initialization attack
            Random rnd = new Random();
            int sumDamage = 0;
            //example 1
            //attacking
            for (int i = 0; i < _CountDecked; i++)
            {
                int currentDamage = rnd.Next(_damageMin, _damageMax);
                shipToAttack.HitPoints -= currentDamage;
                sumDamage += currentDamage;
                if (shipToAttack.HitPoints <= 0)
                {
                    if (shipToAttack is OneDeckedShip)
                    {
                        ((OneDeckedShip)shipToAttack).Dispose();
                    }
                    //other ifs
                    break;
                }
            }
            //ends
            GetDegreeOfDefeatAsDouble(sumDamage, shipToAttack.HitPoints + sumDamage);

            //example 2
            //shipToAttack.HitPoints -= currentDamage * _CountDecked;
            //...other code...//
        }

        /// <summary>
        /// return double view of percents
        /// </summary>
        /// <param name="sumDamage"></param>
        private void GetDegreeOfDefeatAsDouble(int sumDamage, int fullHP)
        {
            ResultLastAttack = Convert.ToString(Math.Round(sumDamage / (double)fullHP, 3));
        }

        //virtual class, doesn't usable at this project needs to import a interface
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _safeHandle?.Dispose();
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BaseShip()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class OneDeckedShip : BaseShip
    {
        private bool disposedValue;
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public OneDeckedShip(int HitPoints, int DamageMin, int DamageMax, Team Team)
            : base(HitPoints, DamageMin, DamageMax, Team)
        {
            _CountDecked = 1; //OneDecked(may use as k)
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _safeHandle?.Dispose();
                }
                base.Dispose(disposing);
                disposedValue = true;
                _safeHandle.Close();
            }
        }
        public new void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
