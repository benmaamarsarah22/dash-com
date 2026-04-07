using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain.Entity;
using System;
using System.Linq;

public class MoisClotureBusinessService : GenericBusinessService<MoisCloture, int>
{
    public MoisClotureBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    //   Vérifier existence mois
    public bool ExisteMois(int annee, int mois)
    {
        return _repository.GetSingle(x => x.Annee == annee && x.Mois == mois) != null;
    }

    //   Vérifier si mois clôturé
    public bool EstCloture(int annee, int mois)
    {
     
        return _repository.GetSingle(x => x.Annee == annee && x.Mois == mois && x.IsCloture) != null;
    }

    //  OUVERTURE MANUELLE DU MOIS
    public void OuvrirMois(int annee, int mois, string userId)
    {
        var date = new DateTime(annee, mois, 1);
        var moisPrecedent = date.AddMonths(-1);

        var precedent = _repository.GetSingle(x =>
            x.Annee == moisPrecedent.Year &&
            x.Mois == moisPrecedent.Month);

        //  CAS 1 : premier mois
        bool premierMois = precedent == null;

        //   CAS 2 : mois précédent non clôturé
        if (!premierMois && !precedent.IsCloture)
        {
            throw new BusinessException("⛔ يجب غلق الشهر السابق قبل فتح شهر جديد");
        }

        //  Déjà ouvert
        if (ExisteMois(annee, mois))
        {
            throw new BusinessException("⛔ الشهر مفتوح بالفعل");
        }

        //  Création du mois (OUVERT)
        _repository.Add(new MoisCloture
        {
            Annee = annee,
            Mois = mois,
            IsCloture = false,
            DateCloture = null,
            CloturePar = null,
        });

        _unitOfWork.SaveChanges();
    }

    // ✅ 🔥 CLÔTURE MANUELLE
    public void CloturerMois(int annee, int mois, string userId)
    {
        var exercice = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);

        if (exercice == null)
        {
            throw new BusinessException("⛔ الشهر غير موجود");
        }

        if (exercice.IsCloture)
        {
            throw new BusinessException("⛔ الشهر مغلق بالفعل");
        }

        exercice.IsCloture = true;
        exercice.DateCloture = DateTime.Now;
        exercice.CloturePar = userId;

        _repository.Update(exercice);
        _unitOfWork.SaveChanges();
    }

    //   REOUVERTURE (DG seulement)
    public void ReouvrirMois(int annee, int mois, bool isDG)
    {
        if (!isDG)
        {
            throw new BusinessException("⛔ فقط الإدارة العامة يمكنها إعادة فتح الشهر");
        }

        var exercice = _repository.GetSingle(x => x.Annee == annee && x.Mois == mois);

        if (exercice == null)
        {
            throw new BusinessException("⛔ الشهر غير موجود");
        }

        exercice.IsCloture = false;
        exercice.DateCloture = null;
        exercice.CloturePar = null;

        _repository.Update(exercice);
        _unitOfWork.SaveChanges();
    }

    //   Vérifier mois ouvert (utilisé dans Activité)
    public void VerifierMoisOuvert(DateTime date)
    {
        var mois = _repository.GetSingle(x => x.Annee == date.Year && x.Mois == date.Month);

        if (mois == null)
        {
            throw new BusinessException("⛔ يجب فتح الشهر قبل إضافة نشاط");
        }

        if (mois.IsCloture)
        {
            throw new BusinessException("⛔ هذا الشهر مغلق");
        }
    }
}
 
    



