# finitautomat
Construction of a finite automaton by a regular grammar

Construction of a finite automaton by a regular grammar.
Task: 
To develop a software tool that realizes the following functions: 
- input an arbitrary formal grammar from the keyboard and check if it belongs to the class of regular grammars; 
- construction of a finite automaton based on the given regular grammar; 
- transformation of a nondeterministic finite automaton to a deterministic finite automaton; 
- displaying the graph of the resulting finite automaton on the screen. 
- finite automaton description.
Input data:
- List of terminals, non-terminals.
- The starting symbol of the grammar.
- Output rules.
Algorithm:
The algorithm starts by initializing the start and end states of the automaton. Then the grammar rules are processed, where the right parts of the rules are replaced by the corresponding transitions in the automaton. Transitions between states of the automaton are then created according to the grammar rules. After that, it is checked whether the automaton is deterministic and a directed graph corresponding to the automaton is constructed.
The algorithm constructs a deterministic finite automaton (DFA) based on a nondeterministic finite automaton (NFA). First, an empty dictionary is created to contain the transitions for each state of the DKA. Then the initial states of the DKA are created and one of them is chosen as the initial state. Then, in a loop, a set of transitions for each unanalyzed state from the NCA is constructed for each input symbol, then these transitions are combined and a new DKA state is created. If this new state has not yet been added to the dictionary, it is added to the queue of unanalyzed states. At the end, "empty" and "unreachable" states are removed from the dictionary. A directed graph is constructed.
Result:
Description of a Non-deterministic finite automaton + graph 
Description of Deterministic finite automaton + graph

Построение конечного автомата по регулярной грамматике.
Задача: 
Разработать программное средство, реализующее следующие функции: 
- ввод с клавиатуры произвольной формальной грамматики и проверка ее принадлежности к классу регулярных грамматик; 
- построение конечного автомата по заданной регулярной грамматике; 
- преобразование недетерминированного конечного автомата в детерминированный конечный автомат; 
- вывод на экран графа полученного конечного автомата. 
- описание конечного автомата.
Входные данные:
- Список терминалов, нетерминалов.
- Начальный символ грамматики.
- Выходные правила.
Алгоритм:
Алгоритм начинается с инициализации начального и конечного состояний автомата. Затем обрабатываются правила грамматики, где нужные части правил заменяются соответствующими переходами в автомате. Затем создаются переходы между состояниями автомата в соответствии с правилами грамматики. После этого проверяется, является ли автомат детерминированным, и строится соответствующий автомату направленный граф.
Алгоритм строит детерминированный конечный автомат (DFA) на основе недетерминированного конечного автомата (NFA). Сначала создается пустой словарь, содержащий переходы для каждого состояния ДКА. Затем создаются начальные состояния ДКА, и одно из них выбирается в качестве начального. Затем в цикле для каждого входного символа строится набор переходов для каждого неанализированного состояния из NCA, после чего эти переходы объединяются и создается новое состояние DKA. Если это новое состояние еще не было добавлено в словарь, оно добавляется в очередь неанализированных состояний. В конце из словаря удаляются "пустые" и "недостижимые" состояния. Строится направленный граф.
Результат:
Описание недетерминированного конечного автомата + граф 
Описание детерминированного конечного автомата + граф
