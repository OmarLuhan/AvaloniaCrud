using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crud.Models;
using Crud.Services;

namespace Crud.ViewModels;

public partial class PersonViewModel : ObservableObject
{
    private readonly IPersonService _service;
    
    public ObservableCollection<Person> People { get; } = [];
    
    //Las propiedades se generan automáticamente con [ObservableProperty]
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCommand), nameof(UpdateCommand))]
    private string? _name;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BirthDateOffset))] 
    private DateTime _birthDate = DateTime.Now;
    public DateTimeOffset BirthDateOffset
    {
        get => BirthDate;
        set => BirthDate = value.DateTime;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCommand), nameof(UpdateCommand))]
    private string? _gender;

    [ObservableProperty]
    private bool _acceptTerms;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand), nameof(DeleteCommand))]
    private Person? _personSelected;

    public PersonViewModel(IPersonService service)
    {
        _service = service;
        _ = LoadPeople();
    }
    
    // Los comandos se generan automáticamente con [RelayCommand]
    [RelayCommand(CanExecute = nameof(Validate))]
    private async Task Add()
    {
        var newPerson = new Person
        {
            Name = Name?.Trim(),
            BirthDate = BirthDate,
            Gender = Gender,
            AcceptTerms = AcceptTerms,
        };
        
        await _service.Add(newPerson);
        await LoadPeople();
        CleanFields();
    }

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task Update()
    {
        PersonSelected!.Name = Name?.Trim();
        PersonSelected.BirthDate = BirthDate;
        PersonSelected.Gender = Gender?.Trim();
        PersonSelected.AcceptTerms = AcceptTerms;
        
        await _service.Update(PersonSelected);
        await LoadPeople();
        CleanFields();
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task Delete()
    {
        await _service.Delete(PersonSelected!.Id);
        await LoadPeople();
        CleanFields();
    }

    [RelayCommand]
    private void CleanFields()
    {
        Name = string.Empty;
        BirthDate = DateTime.Now;
        Gender = string.Empty;
        AcceptTerms = false;
        PersonSelected = null;
    }

    private bool Validate() => 
        !string.IsNullOrWhiteSpace(Name) && 
        !string.IsNullOrWhiteSpace(Gender);

    partial void OnPersonSelectedChanged(Person? oldValue, Person? newValue)
    {
        if (newValue != null)
        {
            Name = newValue.Name;
            BirthDate = newValue.BirthDate;
            Gender = newValue.Gender;
            AcceptTerms = newValue.AcceptTerms;
        }
        else
        {
            CleanFields();
        }
    }
    private bool CanUpdate() => PersonSelected != null && Validate();
    private bool CanDelete() => PersonSelected != null;

    private async Task LoadPeople()
    {
        try
        {
            var people = await _service.GetAll();
            People.Clear();
            foreach (var p in people)
            {
                People.Add(p);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading people: {ex.Message}");
        }
    }
}