﻿using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLight.PhoneBook.Dto;
using BusinessLight.PhoneBook.Mvc.Extensions;
using BusinessLight.PhoneBook.Mvc.ViewModels;
using BusinessLight.PhoneBook.Service;
using BusinessLight.Validation;


namespace BusinessLight.PhoneBook.Mvc.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactCrudService _contactCrudService;

        public ContactsController(ContactCrudService contactCrudService)
        {
            if (contactCrudService == null)
            {
                throw new ArgumentNullException("contactCrudService");
            }

            _contactCrudService = contactCrudService;
        }

        public ActionResult Index()
        {
            return View(new SearchContactViewModel());
        }

        [HttpPost]
        public ActionResult Index(SearchContactViewModel searchContactViewModel)
        {
            try
            {
                searchContactViewModel.PagedResult = _contactCrudService.Search(searchContactViewModel.PagedFilter);
            }
            catch (ValidationException ex)
            {
                ModelState.AddValidationErrors(ex);
            }

            return View(searchContactViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactDetailDto contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contactCrudService.CreateContact(contact);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddValidationErrors(ex);
                }
            }

            return View(contact);
        }

        public ActionResult Edit(Guid id)
        {
            var contactDetail = _contactCrudService.GetDetail(id);
            if (contactDetail == null)
            {
                return HttpNotFound();
            }
            return View(contactDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactDetailDto contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contactCrudService.UpdateContact(contact);
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddValidationErrors(ex);
                }
            }

            contact.ContactInfos = _contactCrudService.GetContactInfosForContact(contact.Id).ToList();
            return View(contact);
        }

        public ActionResult Delete(Guid id)
        {
            var contactDetail = _contactCrudService.GetDetail(id);
            if (contactDetail == null)
            {
                return HttpNotFound();
            }
            return View(contactDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            _contactCrudService.DeleteContact(id);
            return RedirectToAction("Index");
        }
    }
}
