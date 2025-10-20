using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Crud.Helpers;
using Crud.Models;
using Crud.Services;

namespace Crud.ViewModels;

public sealed class PersonViewModel:INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly IPersonService _service;
    public ObservableCollection<Person> People { get; } = [];
    public AsyncRelayCommand AddCommand { get; }
    public AsyncRelayCommand UpdateCommand { get; }
    public AsyncRelayCommand DeleteCommand { get; }
    public ICommand CleanCommand { get; }
    public PersonViewModel(IPersonService service)
    {
        _service = service;
        
        AddCommand = new AsyncRelayCommand(Add, Validate);
        UpdateCommand = new AsyncRelayCommand(Update, () => PersonSelected != null && Validate());
        DeleteCommand = new AsyncRelayCommand(Delete, () => PersonSelected != null);
        CleanCommand = new RelayCommand(CleanFields);
        
        _ = LoadPeople();
        CleanFields();
    }
    private string? _name;
    private DateTime _birthDate = DateTime.Now;
    private string? _gender;
    private bool _acceptTerms;
    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
            AddCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
        }
    }

    private DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            _birthDate = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(BirthDateOffset)); 
        }
    }
    public DateTimeOffset? BirthDateOffset
    {
        get => _birthDate;
        set
        {
            if (value.HasValue)
            {
                BirthDate = value.Value.DateTime;
            }
        }
    }
    public string? Gender
    {
        get => _gender;
        set
        {
            _gender = value;
            OnPropertyChanged();
            AddCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
           
        }
    }
    public bool AcceptTerms
    {
        get => _acceptTerms;
        set
        {
            _acceptTerms = value;
            OnPropertyChanged();
            
        }
    }
    
    private Person? _personSelected;
    
    public Person? PersonSelected
    {
        get => _personSelected;
        set
        {
            if (_personSelected == value) return;
            _personSelected = value;
            OnPropertyChanged();
            UpdateCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            if (value != null)
            {
                Name= value.Name;
                BirthDate = value.BirthDate;
                Gender = value.Gender;
                AcceptTerms = value.AcceptTerms;
            }
            else
            {
                CleanFields();
            }
        }
    }

    private bool Validate() =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(Gender);

    private async Task Add()
    {
        if (!Validate()) return;

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

    private async  Task Update()
    {
        if (PersonSelected == null || !Validate()) return;
        
        PersonSelected.Name = Name?.Trim();
        PersonSelected.BirthDate= BirthDate;
        PersonSelected.Gender = Gender?.Trim();
        PersonSelected.AcceptTerms = AcceptTerms;
        
        await _service.Update(PersonSelected);
        await LoadPeople();
        CleanFields();
    }
    private async Task LoadPeople()
    {
        People.Clear();
        foreach (var p in await _service.GetAll())
        {
            People.Add(p);
        }
    }

    private async Task Delete()
    {
        if (PersonSelected == null) return;
        await _service.Delete(PersonSelected.Id);
        await LoadPeople();
        CleanFields();
    }
    private void CleanFields()
    {
        Name = string.Empty;
        BirthDate = DateTime.Now;
        Gender = string.Empty;
        AcceptTerms = false;
        PersonSelected = null;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}