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

        public bool EstCloture(int annee, int mois)
        {
            var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
            return row != null && row.IsCloture;
        }

        public void AssertMoisOuvert(int annee, int mois)
        {
            var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
            if (row == null || row.IsCloture)
                throw new BusinessException("الشهر مغلق. لا يمكن القيام بأي تعديل.");
        }

        public BusinessResult OuvrirMois(int annee, int mois, string userId)
        {
            try
            {
                var existing = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
                if (existing != null)
                    throw new BusinessException("هذا الشهر موجود بالفعل. استخدم إعادة الفتح للشهر المغلق.");

                var hasPrevious = _repository.Count() > 0;
                if (hasPrevious)
                {
                    var prevDate = new DateTime(annee, mois, 1).AddMonths(-1);
                    var prev = _repository.GetSingle(x => x.Annee == prevDate.Year && x.Mois == prevDate.Month);

                    if (prev != null && !prev.IsCloture)
                    {
                        var daysPassed = (DateTime.Now - new DateTime(prevDate.Year, prevDate.Month, 1)).Days;
                        if (daysPassed > 7)
                            throw new BusinessException("الشهر السابق لم يُغلق بعد أسبوع من الفتح. يجب إغلاق الشهر الحالي أولاً.");
                    }
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
                return BuildFailure("خطأ أثناء فتح الشهر. حاول مجدداً.");
            }
            catch (Exception)
            {
                return BuildFailure("حدث خطأ غير متوقع.");
            }
        }

        public BusinessResult CloturerMois(int annee, int mois, string userId)
        {
            try
            {
                var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
                if (row == null)
                    throw new BusinessException("الشهر غير موجود.");

                if (row.IsCloture)
                    throw new BusinessException("الشهر مغلق بالفعل.");

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
                return BuildFailure("خطأ أثناء إغلاق الشهر. حاول مجدداً.");
            }
            catch (Exception)
            {
                return BuildFailure("حدث خطأ غير متوقع.");
            }
        }

        public BusinessResult ReouvrirMois(int annee, int mois, string userId)
        {
            try
            {
                var row = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);
                if (row == null)
                    throw new BusinessException("الشهر غير موجود.");

                if (!row.IsCloture)
                    throw new BusinessException("الشهر مفتوح بالفعل.");

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
                return BuildFailure("خطأ أثناء إعادة فتح الشهر. حاول مجدداً.");
            }
            catch (Exception)
            {
                return BuildFailure("حدث خطأ غير متوقع.");
            }
        }

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




