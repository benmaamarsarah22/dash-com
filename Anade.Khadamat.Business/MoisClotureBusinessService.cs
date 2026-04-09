using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class MoisClotureBusinessService : GenericBusinessService<MoisCloture, int>
    {
        public MoisClotureBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        // ── Query helpers ─────────────────────────────────────────────────────────

        /// <summary>Returns true when the month row exists AND is flagged as closed.</summary>
        public bool EstCloture(int annee, int mois)
        {
            var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
            return row != null && row.IsCloture;
        }

        /// <summary>
        /// Call this from sub-entity lifecycle hooks to block CUD operations.
        /// </summary>
        public void AssertMoisOuvert(int annee, int mois)
        {
            var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);

            if (row == null || row.IsCloture)
                throw new BusinessException(
                    $"Le mois {new DateTime(annee, mois, 1):MMMM yyyy} est clôturé. Aucune modification n'est permise.");
        }

        // ── Ouvrir ───────────────────────────────────────────────────────────────

        public BusinessResult OuvrirMois(int annee, int mois, string userId)
        {
            try
            {
                // Guard: month already exists
                var existing = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
                if (existing != null)
                    throw new BusinessException("Ce mois est déjà créé. Vous ne pouvez pas le rouvrir via cette action (utilisez Réouvrir).");

                // Ensure previous month is closed (skip for the very first row)
                var hasPrevious = _repository.Count() > 0;
                if (hasPrevious)
                {
                    var prevDate = new DateTime(annee, mois, 1).AddMonths(-1);
                    var prev = _repository.GetSingle(x => x.Annee == prevDate.Year && x.Mois == prevDate.Month);

                    if (prev != null && !prev.IsCloture)
                        throw new BusinessException("Le mois précédent doit être clôturé avant d'ouvrir un nouveau mois.");
                }

                _repository.Add(new MoisCloture
                {
                    Annee = annee,
                    Mois = mois,
                    IsCloture = false,
                    CloturePar = null,
                    DateCloture = null
                });

                if (_unitOfWork.SaveChanges() == 0)
                    throw new DataNotUpdatedException();

                return BusinessResult.Success;
            }
            catch (BusinessException ex)
            {
                return BuildFailure(ex.Message);
            }
            catch (DataNotUpdatedException)
            {
                return BuildFailure("Une erreur est survenue lors de l'ouverture du mois. Veuillez réessayer.");
            }
            catch (Exception)
            {
                return BuildFailure("Une erreur inattendue est survenue.");
            }
        }

        // ── Clôturer ─────────────────────────────────────────────────────────────

        public BusinessResult CloturerMois(int annee, int mois, string userId)
        {
            try
            {
                var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);

                if (row == null)
                    throw new BusinessException("Ce mois n'a pas encore été ouvert.");

                if (row.IsCloture)
                    throw new BusinessException("Ce mois est déjà clôturé.");

                row.IsCloture = true;
                row.DateCloture = DateTime.Now;
                row.CloturePar = userId;

                _repository.Update(row);

                if (_unitOfWork.SaveChanges() == 0)
                    throw new DataNotUpdatedException();

                return BusinessResult.Success;
            }
            catch (BusinessException ex)
            {
                return BuildFailure(ex.Message);
            }
            catch (DataNotUpdatedException)
            {
                return BuildFailure("Une erreur est survenue lors de la clôture du mois. Veuillez réessayer.");
            }
            catch (Exception)
            {
                return BuildFailure("Une erreur inattendue est survenue.");
            }
        }

        // ── Réouvrir (DG / Admin uniquement) ─────────────────────────────────────

        public BusinessResult ReouvrirMois(int annee, int mois, string userId)
        {
            try
            {
                var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);

                if (row == null)
                    throw new BusinessException("Ce mois n'existe pas.");

                if (!row.IsCloture)
                    throw new BusinessException("Ce mois est déjà ouvert.");

                row.IsCloture = false;
                row.DateReouverture = DateTime.Now;
                row.ReouvertPar = userId;

                _repository.Update(row);

                if (_unitOfWork.SaveChanges() == 0)
                    throw new DataNotUpdatedException();

                return BusinessResult.Success;
            }
            catch (BusinessException ex)
            {
                return BuildFailure(ex.Message);
            }
            catch (DataNotUpdatedException)
            {
                return BuildFailure("Une erreur est survenue lors de la réouverture du mois. Veuillez réessayer.");
            }
            catch (Exception)
            {
                return BuildFailure("Une erreur inattendue est survenue.");
            }
        }

        // ── Private ───────────────────────────────────────────────────────────────

        private static BusinessResult BuildFailure(string message)
        {
            var result = new BusinessResult(false);
            result.Messages.Add(new MessageResult
            {
                Message = message,
                MessageType = MessageType.Warning
            });
            return result;
        }
    }
}




